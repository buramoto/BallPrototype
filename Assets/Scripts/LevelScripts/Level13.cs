using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level13 : MonoBehaviour
{
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

        // setting all tools to true
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(true);

        // below we are setting RESET Btn & UNDO Btn as InActive
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        // below we are setting PLANK, SPRING, STEEL & WOOD Material to InActive
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(false);


        // Setting Layout for the TOOLKIT-PANEL
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 120;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 120;

        GlobalVariables.heaterCap = 1;
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
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
    }
}
