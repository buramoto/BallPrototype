using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This script handles the reset button. When pressed, the current level should be reloaded
 */
public class ResetButton : MonoBehaviour
{
    // Start is called before the first frame update
    /*private void Start()
    {
        
    }*/

    // Update is called once per frame
    public void execute()
    {   
        // Reset the global variables
        GlobalVariables.plankCounter = 0;
        GlobalVariables.springCounter = 0;
        GlobalVariables.heaterCounter = 0;
        GlobalVariables.attemptCounter++;
        Debug.Log("Attempt after reset: " + GlobalVariables.attemptCounter);
        DungeonMaster.dm.simMode(false, StateReference.resetType.win);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // winScreenExecute();
    }

    public void winScreenExecute()
    {
        GameObject winScreen = GameObject.Find("WinScreen(Clone)");
        Debug.Log("winSreen reference in ResetButton-->" +winScreen);
        winScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
