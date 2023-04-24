using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLevel2 : MonoBehaviour
{
    private int oobCount = 0;
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
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(true);

        // Setting the Heater, STEEL & WOOD MATERIAL of ToolKit Panel to INACTIVE
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(false);

        // To ensure that the buttons isn't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

        GlobalVariables.plankCap = 3;
        GlobalVariables.springCap = 2;
    }
    // Start is called before the first frame update
    void Start()
    {
        // GlobalVariables.levelScore = 0;
        // GameObject scoreText = GameObject.Find("Score_Text");
        // if (scoreText != null)
        // {
        //     scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        // }
        UIBehavior.gameUI.plankCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.plankCap.ToString();
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UIBehavior.gameUI.plankCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.plankCap.ToString();
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
    }

    public void OutOfBounds(string type)
    {
        // Increment the counter for out of bounds
        oobCount++;

        // If the counter is 2, then display the text to assist the player
        if (type == "oob" && oobCount == 2)
        {
            GameObject dtext = GameObject.Find("Level6_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Try aligning Wood Planks & a Spring";
        }
        if (type == "oob" && oobCount == 5)
        {
            GameObject dtext = GameObject.Find("Level6_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Try Rotating the Wood Planks/Spring";
        }
    }
}
