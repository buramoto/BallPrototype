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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void winScreenExecute()
    {
        GameObject winScreen = GameObject.Find("WinScreen");
        winScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
