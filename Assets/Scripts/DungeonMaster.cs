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

    //Objects for controlling start/stop
    private BallProperties ball;

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
            SceneManager.sceneLoaded += sceneLoaded;
            ball = FindObjectOfType<BallProperties>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void sceneLoaded(Scene level, LoadSceneMode mode)
    {

    }

    //This method changes the state of the game from edit to simulaton and vice versa
    public void changeMode()
    {
        if (simulatonMode)
        {
            //BroadcastMessage("endSim");
        }
        else {
            //BroadcastMessage("startSim");
        }
        simulatonMode = !simulatonMode;
    }
}
