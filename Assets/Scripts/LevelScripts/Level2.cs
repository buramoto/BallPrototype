using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level2 : MonoBehaviour
{
    private bool flag = false;
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
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        // below we are setting RESET Btn & UNDO Btn as InActive
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        // below we are setting PLANK & SPRING InActive
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);

        // Setting Layout for the TOOLKIT-PANEL
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 120;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 120;

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

        GlobalVariables.heaterCap = 1;
    }
    void Start()
    {
        GlobalVariables.levelScore = 0;
        flag = false;
        GameObject scoreText = GameObject.Find("Score_Text");
        if (scoreText != null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if(buttonText.text=="Start" && flag==true)
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Drag heater here";
            dtext.GetComponent<TMPro.TextMeshProUGUI>().color = Color.yellow;
            GameObject dtext2 = GameObject.Find("Ice_Plank_Text");
            dtext2.GetComponent<TMPro.TextMeshProUGUI>().text = "Heater heats the Ball only"; 
            dtext2.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            var col = image.GetComponent<Image>().color;
            col.a = 1f;
            image.GetComponent<Image>().color = col;
            var col2 = highlightedArea.GetComponent<Image>().color;
            col2.a = 1f;
            highlightedArea.GetComponent<Image>().color = col2;
        }
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("We are getting collision");
        if (collision.gameObject.name == "Ball" && this.name == "Collider1" && GlobalVariables.attemptCounter==1)
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Stuck?\n Press stop";
            dtext.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
            flag = true;
        }
    }
}
