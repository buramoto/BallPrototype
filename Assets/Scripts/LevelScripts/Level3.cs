using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Level3 : MonoBehaviour
{
    private bool first = true;
    private void Awake()
    {
        // setting all ToolKit & Operation & Control PANEL Btns to ACTIVE
        UIBehavior.gameUI.timer.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        UIBehavior.gameUI.operationPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
        
        // below we are making the Rotate Left & Rotate Right buttons InActive
        UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        // UIBehavior.gameUI.operationPanel.SetActive(false);
        // below we are setting RESET Btn & UNDO Btn as InActive
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);

        // below we are setting PLANK & SPRING InActive
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);

        // Setting Layout for the TOOLKIT-PANEL
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 150;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 150;

        // Setting Layout for the OPERATION-PANEL
        UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 150;
        UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 150;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if(buttonText.text=="Start" && first==false)
        {
            GameObject dtext1 = GameObject.Find("Heater_Text");
            Debug.Log(dtext1);
            dtext1.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Create Heater between ball and plank";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (first && collision.gameObject.name == "Ball" && this.name == "Collider1")
        {
            GameObject dtext = GameObject.Find("Heater_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Oops we are stuck!!! Press stop";
            first = false;
        }
    }


}
