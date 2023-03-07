using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Level6 : MonoBehaviour
{
    // Start is called before the first frame update
    private bool first = true;
    private void Awake()
    {
        GameObject.FindGameObjectsWithTag("MenuBtn")[0].SetActive(true);
        Time.timeScale = 1;
        // Setting all ToolKit & Operation & Control PANEL Btns to ACTIVE
        UIBehavior.gameUI.timer.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        UIBehavior.gameUI.operationPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);

        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        // To ensure that the buttons isn't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;
        
        UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        Debug.Log("Value of attempt counter" + GlobalVariables.attemptCounter);
        if (GlobalVariables.heaterCounter < 1 && buttonText.text == "Start" && GlobalVariables.attemptCounter >= 2)
        {
            GameObject dtext1 = GameObject.Find("Level6_Text");
            Debug.Log(dtext1);
            dtext1.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Place a Heater Right Below the Ball";
        }

        if (buttonText.text == "Start" && GlobalVariables.attemptCounter>=3 && GlobalVariables.plankCounter>0)
        {
            GameObject dtext1 = GameObject.Find("Level6_Text");
            Debug.Log(dtext1);
            dtext1.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Try rotating the plank";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (first && collision.gameObject.name == "Ball" && this.name == "Collider1" && GlobalVariables.plankCounter<1)
        {
            GameObject dtext = GameObject.Find("Level6_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Align Planks to direct ball";
            first = false;
        }
    }
}
