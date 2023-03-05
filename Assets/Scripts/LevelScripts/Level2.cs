using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2 : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 1;
        // GameObject.Find("Timer").SetActive(false);
        UIBehavior.gameUI.timer.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        UIBehavior.gameUI.operationPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        
        UIBehavior.gameUI.toolKitPanel.SetActive(false);
        UIBehavior.gameUI.operationPanel.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);
    }
    void Start()
    {
        /*
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        UIBehavior.gameUI.operationPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.SetActive(false);
        UIBehavior.gameUI.operationPanel.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("We are getting collision");
        if (collision.gameObject.name == "Ball" && this.name == "Collider1")
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Ice Sheets do not affect the ball";
        }
        else if (collision.gameObject.name == "Ball" && this.name == "Collider2")
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "But it may block path to Goal";
        }
    }
}
