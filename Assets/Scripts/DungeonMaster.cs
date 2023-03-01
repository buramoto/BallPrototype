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

    //References to all static objects in scene
    private BallScript[] balls;
    private Plank[] levelPlanks;
    private GoalBlock[] goals;
    private Spring[] levelSprings;
    private ChangeTemperature[] tempElements;
    private string[] tutorialScenes = { "Tutorial_1", "Tutorial_2", "EnemyTutorial" };
    private static string[] scenes = { "Tutorial_1", "Tutorial_2", "EnemyTutorial", "UIDev" };
    public GameObject[] enemyElements;
    public HeartBehavior[] hearts;
    public TMPro.TextMeshProUGUI instructions;

    // variable to store different scene names
    // public string tutorial1 = "Tutorial_1";
    // public string tutorial2 = "Tutorial_2";


    // array to store checkpoint time
    public static float[] timeArray = new float[4];


    // timevalue stores the currentTime and timer is the text gameobject
    public static float timeValue = 0;
    private GameObject timer;

    //Sequence of checkpoints, should be configurable by level
    // private char[] sequence = {'p', 'y', 'w', 'g'};//original
    private char[] sequence;//THIS NEEDS TO CHANGE
    private IDictionary<string,char[]> sequences = new Dictionary<string, char[]>();
    
    public string nextSceneName; //This should be set in the local DM
    public string currentSceneName;
    // private int checkpointcount_tutorial1 = 0;

    //Events
    public delegate void StartSimulation();
    public event StartSimulation StartSim;
    public delegate void StopSimulation(StateReference.resetType type);
    public event StopSimulation StopSim;

    //Settings
    public float rotationSpeed;
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
            sequences.Add("Tutorial_1",tutorial_sequence);
            sequences.Add("Tutorial_2",tutorial_sequence);
            sequences.Add("EnemyTutorial",tutorial_sequence);
            char[] main_level_sequence = {'p','y','w','g'};
            sequences.Add("UIDev",main_level_sequence);
        }
        else
        {
            // dm.sequence = sequence;//Should change
            // dm.nextSceneName = scenes[sceneIndex+1]; //Set the next level scene name
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
        timer = GameObject.Find("Timer");
        Debug.Log("Initialize Level count of ENEMY: " + enemyElements.Length);
        enemyElements = GameObject.FindGameObjectsWithTag("Enemy");
        // Debug.Log("Scene Index" + sceneIndex);
        
    }


    private void Update()
    {
        //Calculating time for every update and updating the text in Timer gameobject
        timeValue += Time.deltaTime;
        float minutes = Mathf.FloorToInt(timeValue / 60);
        float seconds = Mathf.FloorToInt(timeValue % 60);
        //TextMeshProUGUI tm =  timer.GetComponent<TextMeshProUGUI>(); 
        //tm.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
        sequence = sequences[currentSceneName];
        Debug.Log("Initalizing level "+currentSceneName);
        if(currentSceneName == "MainMenu")
        {
            return;
        }
        lives = maxLives;
        balls = FindObjectsOfType<BallScript>();
        levelPlanks = FindObjectsOfType<Plank>();
        goals = FindObjectsOfType<GoalBlock>();

        hearts = FindObjectsOfType<HeartBehavior>();

        enemyElements = GameObject.FindGameObjectsWithTag("Enemy");

        simulationMode = false;
        counter = 0;
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
    public void loadNextLevel()
    {
        if(sceneIndex<scenes.Length-1) {
            timeValue = 0;
            TextMeshProUGUI tm =  timer.GetComponent<TextMeshProUGUI>(); 
            tm.text = "0:00";
            currentSceneName = scenes[sceneIndex];
            sceneIndex++;
            nextSceneName = scenes[sceneIndex];
            SceneManager.LoadScene(nextSceneName);
        }
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
            //instructions.text = "Right Click To Attack Enemy";
            //Trigger start sim event
            StartSim?.Invoke();
        }
        else {
            // initialize time array
            //Debug.LogWarning("Simulation stopped due to "+type.ToString()+"!");
            //instructions.text = "Use The Tools To The Right To Direct The Ball &\nThen Click Start To Begin Ball's Motion";
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
            //Trigger stop sim event
            StopSim?.Invoke(type);

            // Increment attempt counter
            GlobalVariables.attemptCounter++;
            Debug.Log("Attempt after reset FROM DM: " + GlobalVariables.attemptCounter);

            // Change the button text
            GameObject button = GameObject.Find("StartButton");
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = "Start";

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
            //adding goal time to timeArray
            timeArray[counter]=timeValue;

            // Storing the number of player's lives left
            GlobalVariables.livesLeft = lives;

            Debug.Log("Heaters used: " + GlobalVariables.heaterUsed);
            //Display a Win screen
            if (!tutorialScenes.Contains(currentSceneName))
            {
                Debug.Log("GOOGLE");
                SendToGoogle.sendToGoogle.Send();
            }
            checkpoint.SetActive(false);
            GlobalVariables.oobCounter = 0;
            GlobalVariables.wgoCounter = 0;
            GlobalVariables.attemptCounter = 0;
            //From Analytics
            GlobalVariables.plankUsed = 0;
            GlobalVariables.springUsed = 0;
            GlobalVariables.heaterUsed = 0;
            UIBehavior.gameUI.displayWinScreen();
            if(sceneIndex == scenes.Length-1) {
                UIBehavior.gameUI.displayWinScreen();
            } else {
                UIBehavior.gameUI.displayNextLevelScreen(nextSceneName);
            }
        }
        else if(checkpointColor == sequence[counter])
        {
            //Correct, play sound
            //add time value to array
            timeArray[counter]=timeValue;
            //Correct, play sound
            counter++;
        }
        else
        {  
            //Player got the ball to the wrong goal block
            GlobalVariables.wgoCounter = 0;
            simMode(false, StateReference.resetType.wgo);
            // array initialize to empty
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

    public void resetValues(){
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

    }


}
