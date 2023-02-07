using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonMaster : MonoBehaviour
{
    //Static reference so all objects can access the DM
    public static DungeonMaster dm;

    //Game variables
    public bool simulatonMode;
    public GameObject WinScreen;
    private GameObject canvas;

    //Objects for controlling start/stop
    private BallScript ball;

    //Sequence of checkpoints
    private char[] sequence = {'p', 'y', 'w'};
    private byte counter;

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
        ball = FindFirstObjectByType<BallScript>();
        simulatonMode = false;
        canvas = FindObjectOfType<Canvas>().gameObject;
        counter = 0;
    }

    //This method changes the state of the game from edit to simulaton and vice versa
    public void changeMode()
    {
        if (simulatonMode)
        {
            Debug.Log("Simulaton stopped");
            counter = 0;
            ball.stopSim();
        }
        else {
            Debug.Log("Simulation Started");
            ball.startSim();
        }
        simulatonMode = !simulatonMode;
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
            Debug.Log("Inside the if statement");
            var WinSc = Instantiate(WinScreen,canvas.transform.position,Quaternion.identity);
            //WinSc.transform.parent = canvas.transform;
            WinSc.transform.SetParent(canvas.transform);
        }
        else if(checkpointColor == sequence[counter])
        {
            //Correct, play sound
            counter++;
        }
        else
        {
            //Wrong
            //changeMode();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
