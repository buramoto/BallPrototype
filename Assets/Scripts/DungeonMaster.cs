using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DungeonMaster : MonoBehaviour
{
    //Static reference so all objects can access the DM
    public static DungeonMaster dm;


    // variable to store reference to highlighted object
    public GameObject highlightedObject;

    //Game variables
    public bool simulationMode;
    public byte counter;

    //References to all static objects in scene
    private BallScript[] balls;
    private Plank[] levelPlanks;
    private GoalBlock[] goals;
    private Spring[] levelSprings;
    private ChangeTemperature[] tempElements;
    // array to store checkpoint time
    public static float[] timeArray = new float[4];


    // timevalue stores the currenttime and timer is the text gameobject
    public static float timeValue = 0;
    private GameObject timer;

    //Sequence of checkpoints, should be configurable by level
    private char[] sequence = {'p', 'y', 'w'};

    //Events
    public delegate void StartSimulation();
    public event StartSimulation StartSim;
    public delegate void StopSimulation(StateReference.resetType type);
    public event StopSimulation StopSim;

    //Settings
    public float rotationSpeed;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //initalizeLevel();
        //Initializing the timer object
        timer = GameObject.Find("Timer");
    }


    private void Update()
    {
        //Calculating time for every update and updating the text in Timer gameobject
        timeValue += Time.deltaTime;
        float minutes = Mathf.FloorToInt(timeValue / 60);
        float seconds = Mathf.FloorToInt(timeValue % 60);
        TextMeshProUGUI tm =  timer.GetComponent<TextMeshProUGUI>(); 
        tm.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
        Debug.Log("Initalizing level "+scene.name);

        GameObject winScreen = GameObject.Find("WinScreen(Clone)");
        if(winScreen != null){
            winScreen.SetActive(false);
            Destroy(winScreen);
        }
        balls = FindObjectsOfType<BallScript>();
        levelPlanks = FindObjectsOfType<Plank>();
        goals = FindObjectsOfType<GoalBlock>();
        simulationMode = false;
        counter = 0;
        simMode(false, StateReference.resetType.ssb);
        Debug.Log("Initalizing level of Dungeon Master has been Executed");
        // Debug.Log(goals.Length);
        for(int i=0;i<goals.Length;i++) {
            Debug.Log(goals[i].gameObject);
            goals[i].gameObject.SetActive(true);
        }
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
            //Trigger start sim event
            StartSim?.Invoke();
        }
        else {
            // initialize time array
            Debug.LogWarning("Simulation stopped due to "+type.ToString()+"!");
            for (int i = 0; i < levelPlanks.Length; i++)
            {
                levelPlanks[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < goals.Length; i++)
            {
                goals[i].gameObject.SetActive(true);
            }
            for(int i = 0; i < balls.Length; i++)
            {
                balls[i].stopSim();
            }
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
        if(checkpointColor=='g' && counter==3)
        {
            //adding goal time to timeArray
            timeArray[counter]=timeValue;
            //Display a Win screen
            SendToGoogle.sendToGoogle.Send();
            checkpoint.SetActive(false);
            GlobalVariables.attemptCounter = 0;
            UIBehavior.gameUI.displayWinScreen();
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

}
