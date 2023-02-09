using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonMaster : MonoBehaviour
{
    //Static reference so all objects can access the DM
    public static DungeonMaster dm;

    //Game variables
    public bool simulationMode;
    private byte counter;

    //Objects for controlling start/stop
    private BallScript[] balls;
    private Static_Plank[] levelPlanks;
    private GoalBlock[] goals;
    private Spring[] levelSprings;

    //Sequence of checkpoints, should be configurable by level
    private char[] sequence = {'p', 'y', 'w'};

    //Events
    public delegate void StartSimulation();
    public event StartSimulation StartSim;
    public delegate void StopSimulation(StateReference.resetType type);
    public event StopSimulation StopSim;

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
    ///     1) Updating the UI wih appropriate toolkit items (eventually we should make this a json file)
    ///     2) Initalizing the ball variable with this level's ball
    ///     3) Setting the level's timer
    /// </summary>
    private void initalizeLevel(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Initalizing level "+scene.name);
        balls = FindObjectsOfType<BallScript>();
        levelPlanks = FindObjectsOfType<Static_Plank>(); //Change this to regular plank
        goals = FindObjectsOfType<GoalBlock>();
        simulationMode = false;
        counter = 0;
    }

    //This method changes the state of the game from edit to simulaton and vice versa
    public void simMode(bool mode, StateReference.resetType type)
    {
        if (mode == simulationMode)
        {
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
            Debug.LogWarning("Simulation stopped!");
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
        }
        simulationMode = !simulationMode;
    }

    /// <summary>
    /// Scorekeeping- this method should be called when the ball hits a checkpoint.
    /// It will determine if that checkpoint has been hit in the correct sequence then update
    /// the UI as necessary.
    /// </summary>
    public void checkpointHit(char checkpointColor)
    {
        Debug.Log("Counter value" + counter);

        if(checkpointColor=='g' && counter==3)
        {
            //Display a Win screen
            UIBehavior.gameUI.displayWinScreen();
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
}
