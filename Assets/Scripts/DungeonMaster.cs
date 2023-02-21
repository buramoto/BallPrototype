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
    public float resources;

    //References to all static objects in scene
    private BallScript[] balls;
    private Plank[] levelPlanks;
    private GoalBlock[] goals;
    private Spring[] levelSprings;
    private ChangeTemperature[] tempElements;

    //Sequence of checkpoints, should be configurable by level
    private char[] sequence = {'p', 'y', 'w'};

    //Events
    public delegate void StartSimulation();
    public event StartSimulation StartSim;
    public delegate void StopSimulation(StateReference.resetType type);
    public event StopSimulation StopSim;

    //Settings
    public float rotationSpeed;
    public Vector3 smallBallScale;
    public Vector3 mediumBallScale;
    public Vector3 largeBallScale;
    public float smallResourceValue;
    public float mediumResourceValue;
    public float largeResourceValue;
    //Cost variables
    public int plankCost;
    public int springCost;
    public int elementCost;

    /// <summary>
    /// Creates the dm for the first time, or if there is already on (e.g. loading in from a different
    /// level, destroys the object)
    /// </summary>
    private void Awake()
    {
        if(dm == null)
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
    public void checkpointHit(GameObject checkpoint, char checkpointColor, StateReference.ballType ballType)
    {
        Debug.Log("Counter value" + counter);
        if(checkpointColor=='g' && counter==3)
        {
            //Display a Win screen
            checkpoint.SetActive(false);
            UIBehavior.gameUI.displayWinScreen();
            //Calculate resources here
            switch (ballType)
            {
                case StateReference.ballType.small:
                    resources += smallResourceValue;
                    break;
                case StateReference.ballType.medium:
                    resources += mediumResourceValue;
                    break;
                case StateReference.ballType.large:
                    resources += largeResourceValue;
                    break;
            }
            SendToGoogle.sendToGoogle.Send();
            GlobalVariables.attemptCounter = 0;
        }
        else if(checkpointColor == sequence[counter])
        {
            //Correct, play sound
            counter++;
        }
        else
        {  
            //Player got the ball to the wrong goal block
            simMode(false, StateReference.resetType.wgo);
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

    /// <summary>
    /// This method is invoked from one of the 3 size change buttons. For each ball in the scene
    /// it will invoke the change size method on each ball. Scaling is done within ballScript
    /// </summary>
    /// <param name="size"></param>
    public void changeBallSize(StateReference.ballType size)
    {
        if (simulationMode)
        {
            return;
        }
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].changeBallState(size);
        }
    }
}
