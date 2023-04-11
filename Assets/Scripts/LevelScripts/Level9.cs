using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level9 : MonoBehaviour
{
    public static Level9 Level9Reference;

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

        // UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        // UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        // UIBehavior.gameUI.operationPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);

        
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;
        
        // UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        // UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

        GlobalVariables.plankCap = 5;
        GlobalVariables.springCap = 2;
        GlobalVariables.heaterCap = 2;
        
        if (Level9Reference == null)
        {
            Level9Reference = this;
        }
        else
        {

        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.levelScore = 0;
        GameObject scoreText = GameObject.Find("Score_Text");
        if (scoreText != null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }

        UIBehavior.gameUI.plankCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.plankCap.ToString();
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UIBehavior.gameUI.plankCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.plankCap.ToString();
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();   
    }
}
