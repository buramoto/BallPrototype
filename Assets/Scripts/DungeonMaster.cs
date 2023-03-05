using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using Unity.VisualScripting;
using System.Linq;

public class DungeonMaster : MonoBehaviour
{
    //Static reference so all objects can access the DM
    public static DungeonMaster dm;


    // variable to store reference to highlighted object
    public GameObject highlightedObject;

    //Game variables
    public bool simulationMode;
    public byte counter;
    public int lives;
    public static int sceneIndex = 0;
    private bool isLevelOn;

    //References to all static objects in scene
    private BallScript[] balls;
    private Plank[] levelPlanks;
    private GoalBlock[] goals;
    private Spring[] levelSprings;
    private ChangeTemperature[] tempElements;

    // For our reference:
    // Level1: Introduction to Ball, Goal, Start button
    // Level2: Introduction to Ice Planks
    // Level5: Introduction to Springs
    // Level6: Planks and Heater
    // Level7: Planks and Spring
    // Level8: Introduction to enemies
    // Level9: Main Level (Previously "UIDev")
    private string[] tutorialScenes = {"Level1", "Level2", "Level3", "Level4", "Level5", "Level6", "Level7", "Level8"};
    private static string[] scenes = {"Level1", "Level2", "Level3", "Level4", "Level5", "Level6", "Level7", "Level8", "Level9", "Level10", "Level11","Level12"};
    public GameObject[] enemyElements;
    public HeartBehavior[] hearts;
    public TMPro.TextMeshProUGUI instructions;


    // array to store checkpoint time
    public static float[] timeArray = new float[4];


    // timevalue stores the currentTime and timer is the text gameobject
    public static float timeValue = 0;

    //Sequence of checkpoints, should be configurable by level
    // private char[] sequence = {'p', 'y', 'w', 'g'};//original
    //private char[] sequence;//THIS NEEDS TO CHANGE
    //private IDictionary<string,char[]> sequences = new Dictionary<string, char[]>();
    
    public string nextSceneName; //This should be set in the local DM
    public string currentSceneName;
    // private int checkpointcount_tutorial1 = 0;

    //Events
    public delegate void StartSimulation();
    public event StartSimulation StartSim;
    public delegate void StopSimulation(StateReference.resetType type);
    public event StopSimulation StopSim;

    //Settings
    public int maxLives;

    /// <summary>
    /// Creates the dm for the first time, or if there is already on (e.g. loading in from a different
    /// level, destroys the object)
    /// </summary>
    private void Awake()
    {
        if (dm == null)
        {
            DontDestroyOnLoad(gameObject);
            dm = this;
            SceneManager.sceneLoaded += initalizeLevel;
            char[] tutorial_sequence = {'g'};
            //sequences.Add("Tutorial_1",tutorial_sequence);
            //sequences.Add("Tutorial_2",tutorial_sequence);
            //sequences.Add("EnemyTutorial",tutorial_sequence);
            char[] main_level_sequence = {'p','y','w','g'};
            //sequences.Add("UIDev",main_level_sequence);
        }
        else
        {
            // dm.sequence = sequence;//Should change
            dm.nextSceneName = nextSceneName; //Set the next level scene name
            // dm.currentSceneName = scenes[sceneIndex];
            Destroy(gameObject);
        }
        maxLives = 2;
    }

    private void Start()
    {
        //Debug.Log("Enemy Objects found on start of Dungeon mAster"+GameObject.FindGameObjectsWithTag("Enemy").Length);

        //initalizeLevel();
        //Initializing the timer object
        //instructions = Get;
        Debug.Log("Initialize Level count of ENEMY: " + enemyElements.Length);
        enemyElements = GameObject.FindGameObjectsWithTag("Enemy");
        // Debug.Log("Scene Index" + sceneIndex);
        
    }


    private void Update()
    {
        //Calculating time for every update and updating the text in Timer gameobject
        // if(currentSceneName != "Level1" && currentSceneName != "Level2" && SceneManager.GetActiveScene().name != "MainMenu") {
            if(isLevelOn) {
                timeValue += Time.deltaTime;
                float minutes = Mathf.FloorToInt(timeValue / 60);
                float seconds = Mathf.FloorToInt(timeValue % 60);
                UIBehavior.gameUI.updateTimer(minutes,seconds);
            }
        // }
    }


    /// <summary>
    /// This method initalizes the level for play after loading a scene
    /// These are its responsibilities: 
    ///     1) Resetting the level
    ///     2) Initalizing the DM variable with this level's objects
    ///     3) Setting the level's timer
    /// </summary>
    private void initalizeLevel(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        //sequence = sequences[currentSceneName];
        Debug.Log("Initalizing level "+currentSceneName);
        if(currentSceneName == "MainMenu")
        {
            return;
        }
        isLevelOn = true;
        lives = maxLives;
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Start";
        Debug.Log("From DM:"+buttonText.text);
        UIBehavior.gameUI.changeButtonColor(false);
        balls = FindObjectsOfType<BallScript>();
        levelPlanks = FindObjectsOfType<Plank>();
        goals = FindObjectsOfType<GoalBlock>();

        hearts = FindObjectsOfType<HeartBehavior>();

        enemyElements = GameObject.FindGameObjectsWithTag("Enemy");

        simulationMode = false;
        counter = 0;
        Debug.Log("IN initialize level setting to 0");
        GlobalVariables.levelScore = 0;
        GameObject scoreText = GameObject.Find("Score_Text");
        if(scoreText!=null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }
        simMode(false, StateReference.resetType.ssb);
        for(int i=0;i<goals.Length;i++) {
            Debug.Log(goals[i].gameObject);
            goals[i].gameObject.SetActive(true);
        }

        for(int i=0; i<enemyElements.Length; i++)
        {
            enemyElements[i].SetActive(true);
        }
    }

    //Scene loads
    /// <summary>
    /// Loads the next scene
    /// </summary>
    public void loadNextLevel(string levelName = null)
    {
        if(levelName == null)
        {
            //Win screen here
            return;
        }
        timeValue = 0;
        //TextMeshProUGUI tm =  timer.GetComponent<TextMeshProUGUI>(); 
        //tm.text = "0:00";
        currentSceneName = nextSceneName;
        SceneManager.LoadScene(levelName);
    }
    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //This method changes the state of the game from edit to simulaton mode. Stopping requires type of stop, starting requires resetType.start
    public void simMode(bool mode, StateReference.resetType type)
    {
        if(highlightedObject!=null){
            RemoveHighlightFromObject();
        }
        
        if (mode == simulationMode)
        {
            GameObject button = GameObject.Find("StartButton");
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = "Start";
            UIBehavior.gameUI.changeButtonColor(false);
            // below function is called so that INITIALLY OPERATION BUTTONS are INACTIVE
            UIBehavior.gameUI.setOperationInactive();

            StopSim?.Invoke(type);
            return;
        }
        if (mode) { 
            Debug.LogWarning("Simulaton started!");
            counter = 0;
            for(int i = 0; i < balls.Length; i++)//The ball's start and stop statements should be moved to events
            {
                balls[i].startSim();
            }
            //Trigger start sim event
            StartSim?.Invoke();
        }
        else {
            // initialize time array
            //Debug.LogWarning("Simulation stopped due to "+type.ToString()+"!");
            for (int i = 0; i < levelPlanks.Length; i++)
            {
                levelPlanks[i].gameObject.SetActive(true);
                // if(levelPlanks[i].gameObject.name == "Plank(Clone)" && levelPlanks[i].gameObject.GetComponent<Plank>().editable == true && levelPlanks[i].GameObject.GetComponent<Plank>().hasCollided == true)
                // {
                //     levelPlanks[i].gameObject.GetComponent<Plank>().hasCollided = false;
                // }
            }
            for (int i = 0; i < goals.Length; i++)
            {
                goals[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].stopSim();
            }
            GlobalVariables.levelScore = 0;
            Debug.Log("IN stop sim reset to 0");
            GlobalVariables.levelScore = 0;
            GameObject scoreText = GameObject.Find("Score_Text");
            if (scoreText != null)
            {
                scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
            }
            Debug.Log("====> Count of Enemy_Elements: " + enemyElements.Length);
            Debug.Log("====> Count of Hearts: " + hearts.Length);

            for (int i = 0; i < enemyElements.Length; i++)
            {
                enemyElements[i].SetActive(true);
            }

            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].gameObject.SetActive(true);
            }
            //From Analytics
            //resetValues();
            lives = maxLives;
            SendToGoogle.sendToGoogle.resetGlobalVariables("ModeChange");
            //Trigger stop sim event
            StopSim?.Invoke(type);

            // Increment attempt counter
            GlobalVariables.attemptCounter++;
            Debug.Log("Attempt after reset FROM DM: " + GlobalVariables.attemptCounter);

            // Change the button text
            GameObject button = GameObject.Find("StartButton");
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = "Start";
            UIBehavior.gameUI.changeButtonColor(false);
            // GlobalVariables.heaterCoordinates = "";

        }
        simulationMode = !simulationMode;
    }

    /// <summary>
    /// Scorekeeping- this method should be called when the ball hits a checkpoint.
    /// It will determine if that checkpoint has been hit in the correct sequence then update
    /// the UI as necessary.
    /// </summary>
    public void checkpointHit(GameObject checkpoint, char checkpointColor)
    {
        Debug.Log("Counter value" + counter);
        if(checkpointColor=='g')
        {
            // Stopping the ball once it hits the checkpoint
            // balls[0].gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            // DungeonMaster.dm.freezeBall(0);
            Time.timeScale = 0;
            //adding goal time to timeArray
            //timeArray[counter]=timeValue;
            isLevelOn = false;
            GlobalVariables.levelScore += 500;
            Debug.Log("Checking"+GlobalVariables.levelScore);
            GameObject scoreText = GameObject.Find("Score_Text");
            Debug.Log("This is the score go" + scoreText);
            if (scoreText != null)
            {
                scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
            }
            // Storing the number of player's lives left
            GlobalVariables.livesLeft = lives;

            Debug.Log("Heaters used: " + GlobalVariables.heaterUsed);
            //Display a Win screen

            //current scene name
            Scene currentScene = SceneManager.GetActiveScene();
            GlobalVariables.sceneName = currentScene.name;
            Debug.Log("Scene Name: " + GlobalVariables.sceneName);

            //ANALYTICS
            if (GlobalVariables.sceneName != "Level1" && GlobalVariables.sceneName != "Level2")
            {
                Debug.Log("GOOGLE");
                SendToGoogle.sendToGoogle.Send();
            }
            // commenting below line so that checkpoint is not removed from the screen if it is the FINISH LINE
            
            //checkpoint.SetActive(false);
            


            // GlobalVariables.oobCounter = 0;
            // GlobalVariables.wgoCounter = 0;
            // GlobalVariables.attemptCounter = 0;
            // From Analytics
            // GlobalVariables.plankUsed = 0;
            // GlobalVariables.springUsed = 0;
            // GlobalVariables.heaterUsed = 0;
            SendToGoogle.sendToGoogle.resetGlobalVariables("New Level");
            // GlobalVariables.heaterCoordinates = "";

            if(currentSceneName == scenes[scenes.Length-1]) {
                UIBehavior.gameUI.displayWinScreen();
            } else {
                UIBehavior.gameUI.displayNextLevelScreen(nextSceneName);
            }
        }
        else
        {
            //Correct, play sound
            //add time value to array
            timeArray[counter]=timeValue;
            //Correct, play sound
            counter++;
            if(checkpointColor=='p')
            {

                GlobalVariables.levelScore += GlobalVariables.scoreDict[checkpoint.GetComponent<GoalBlock>().goalColor];
                Debug.Log("added 100 Score is :-" + GlobalVariables.levelScore);
            }
            else if(checkpointColor=='y')
            {
                GlobalVariables.levelScore += GlobalVariables.scoreDict[checkpoint.GetComponent<GoalBlock>().goalColor];
                Debug.Log("added 200 Score is :-" + GlobalVariables.levelScore);
            }
            else if(checkpointColor=='w')
            {
                GlobalVariables.levelScore += GlobalVariables.scoreDict[checkpoint.GetComponent<GoalBlock>().goalColor];
                Debug.Log("added 300 Score is :-" + GlobalVariables.levelScore);
            }
            GameObject scoreText = GameObject.Find("Score_Text");
            Debug.Log("This is the score go" + scoreText);
            if (scoreText != null)
            {
                scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
            }
        }
    }

    public void RemoveHighlightFromObject(){
        if(highlightedObject!=null){
            highlightedObject.GetComponentInChildren<Outline>().enabled =false;
            highlightedObject = null;
            UIBehavior.gameUI.setOperationInactive();
        }
    }

    public void HighlightObject(GameObject currentInstance){
        // Debug.Log("---- DM:" +currentInstance);
        if(currentInstance.CompareTag("Plank") || currentInstance.CompareTag("Spring") || currentInstance.CompareTag("TempChange")){
                    UIBehavior.gameUI.setOperationActive(currentInstance);

                    if( highlightedObject!=null && currentInstance != highlightedObject){
                        highlightedObject.GetComponentInChildren<Outline>().enabled = false;
                        currentInstance.GetComponentInChildren<Outline>().enabled = true;
                        highlightedObject = currentInstance;
                    }
                    else if(currentInstance == highlightedObject){
                        // Debug.Log("In currentInstance == highlightedObject");
                        // do nothing as previously highlighted object is same as the currently clicked object
                    }
                    else if(highlightedObject ==null){
                        // Debug.Log("DM ---> "+currentInstance.GetComponentInChildren<Outline>());
                        currentInstance.GetComponentInChildren<Outline>().enabled =true;
                        highlightedObject = currentInstance;
                    }
                    
        }
        else{
            if(highlightedObject!=null){

                highlightedObject.GetComponentInChildren<Outline>().enabled =false;
            }
        }
    }

    /*public void resetValues(){
        GameObject[] plank = GameObject.FindGameObjectsWithTag("Plank");
        foreach (GameObject p in plank)
        {
            Plank script = p.GetComponent<Plank>();
            if(script!=null && script.editable){

                Debug.Log(p.transform.position);
                script.hasCollided = false;

            }   
        }
        GameObject[] spring = GameObject.FindGameObjectsWithTag("Spring");
        foreach (GameObject s in spring)
        {
            Spring script = s.GetComponent<Spring>();
            if(script!=null && script.editable){
                script.hasCollided = false;

            }   
        }
        GameObject[] heater = GameObject.FindGameObjectsWithTag("TempChange");
        foreach (GameObject h in heater)
        {
            ChangeTemperature script = h.GetComponent<ChangeTemperature>();
            if(script!=null && script.editable){
                script.hasCollided = false;

            }   
        }
        GlobalVariables.plankUsed = 0;
        GlobalVariables.springUsed = 0;
        GlobalVariables.heaterUsed = 0;
        // GlobalVariables.heaterCoordinates = "";
    }*/

    // becuase we have a balls[] and in future we might have multiple balls,
    // hence this function take the index of the particular ball which is to be frozen
    public void freezeBall(int index)
    {
        balls[index].gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }


}
