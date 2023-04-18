using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level14 : MonoBehaviour
{
    //private bool flag = false;
    public GameObject image;
    public GameObject highlightedArea;
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject.FindGameObjectsWithTag("MenuBtn")[0].SetActive(true);
        Time.timeScale = 1;
        // Setting all ToolKit & Operation & Control PANEL Btns to ACTIVE
        UIBehavior.gameUI.timer.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        // UIBehavior.gameUI.operationPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(true);

        // UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        // UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        // UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);

        // below we are making the Rotate Left & Rotate Right buttons InActive
        // UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        // UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        // below we are setting RESET Btn & UNDO Btn as InActive
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        // below we are setting PLANK, STEEL & WOOD Material InActive
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(false);

        // Setting Layout for the TOOLKIT-PANEL
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

        // Setting Layout for the OPERATION-PANEL
        // UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 120;
        // UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 120;
/*
        // Find the arrow and make it transparent
        image = GameObject.Find("Arrow");
        var col = image.GetComponent<Image>().color;
        col.a = 0;
        image.GetComponent<Image>().color = col;

        // Find the highlighted area and make it transparent
        highlightedArea = GameObject.Find("Highlighted area");
        var col2 = highlightedArea.GetComponent<Image>().color;
        col2.a = 0;
        highlightedArea.GetComponent<Image>().color = col2;
*/
        GlobalVariables.springCap = 5;
        GlobalVariables.heaterCap = 5;

    }
    void Start()
    {
        GlobalVariables.levelScore = 0;
       // flag = false;
        GameObject scoreText = GameObject.Find("Score_Text");
        if (scoreText != null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }
        /*
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        UIBehavior.gameUI.operationPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.SetActive(false);
        UIBehavior.gameUI.operationPanel.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);
        */
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        /*if(buttonText.text=="Start" && flag==true)
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Click on Heater\n Drag it on the box below";
            GameObject dtext2 = GameObject.Find("Ice_Plank_Text");
            dtext2.GetComponent<TMPro.TextMeshProUGUI>().text = "Remember: Heater heats the Ball & is one time use only"; 
            dtext2.GetComponent<TMPro.TextMeshProUGUI>().color = Color.yellow;
            var col = image.GetComponent<Image>().color;
            col.a = 0.3f;
            image.GetComponent<Image>().color = col;
            var col2 = highlightedArea.GetComponent<Image>().color;
            col2.a = 0.3f;
            highlightedArea.GetComponent<Image>().color = col2;
        }*/
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Debug.Log("We are getting collision");
        if (collision.gameObject.name == "Ball" && this.name == "Collider1" && GlobalVariables.attemptCounter==1)
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Ice Sheets do not affect the ball";
        }
        else if (collision.gameObject.name == "Ball" && this.name == "Collider2" && GlobalVariables.attemptCounter==1)
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "But it may block path to Goal";
        }
        else if (collision.gameObject.name == "Ball" && this.name == "Collider3" && GlobalVariables.attemptCounter==1)
        {
            GameObject dtext = GameObject.Find("Ice_Plank_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Stuck?\n Press stop";
            dtext.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
            flag = true;
        }*/
        // else if (collision.gameObject.name == "Ball" && this.name == "Collider3" && flag == true)
        // {
        //     GameObject dtext = GameObject.Find("Level_Text");
        //     Debug.Log(dtext);
        //     dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Click on Heater\n Drag it between Ball & Ice Sheet";
        //     GameObject dtext2 = GameObject.Find("Ice_Plank_Text");
        //     dtext2.GetComponent<TMPro.TextMeshProUGUI>().text = "Remember: Heater heats the Ball & is one time use only"; 
        // }
    }
}
