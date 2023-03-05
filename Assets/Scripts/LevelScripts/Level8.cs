using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level8 : MonoBehaviour
{
    public static Level8 Level8Reference;
    // private int kbeCount = 0;

    private void Awake()
    {
        Time.timeScale = 1;
        // Set the timer, tool kit panel, operation panel, and control panel to active
        UIBehavior.gameUI.timer.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        UIBehavior.gameUI.operationPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        
        // Set the tool kit panel, operation panel and reset buttons to inactive
        UIBehavior.gameUI.toolKitPanel.SetActive(false);
        UIBehavior.gameUI.operationPanel.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);

        if (Level8Reference == null)
        {
            Level8Reference = this;
        }
        else
        {

        }
        if (GlobalVariables.kbeCounter == 0)
        {
            GameObject dtext = GameObject.Find("Level8_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Right Click to Attack Enemies when the ball is moving";
        }
        if (GlobalVariables.kbeCounter > 0 && GlobalVariables.kbeCounter < 2)
        {
            GameObject dtext = GameObject.Find("Level8_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: A Sword always appears from right";
        }
        if (GlobalVariables.kbeCounter  == 2)
        {
            GameObject dtext = GameObject.Find("Level8_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Note: you only have 2 lives";
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void KillByEnemy(string type)
    {
        // Increment the counter for kill by enemy
        // kbeCount++;

        // If the counter is 2, then display the text to assist the player
       
    }
}
