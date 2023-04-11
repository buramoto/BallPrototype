using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level4 : MonoBehaviour
{
    // Start is called before the first frame update
    private int oobCount = 0;
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
        
        // Setting the spring and heater buttons of the ToolKit Panel to INACTIVE
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        // To ensure that the buttons aren't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 120;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 120;

        // UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        // UIBehavior.gameUI.operationPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

        GlobalVariables.plankCap = 3;
    }
    void Start()
    {
        GlobalVariables.levelScore = 0;
        GameObject scoreText = GameObject.Find("Score_Text");
        if (scoreText != null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }
        UIBehavior.gameUI.plankCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.plankCap.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UIBehavior.gameUI.plankCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.plankCap.ToString();
    }

    public void OutOfBounds(string type)
    {
        // Increment the counter for out of bounds
        oobCount++;
        Debug.Log("Came inside the function");
         // If the counter is 2, then display the text to assist the player
         if (type == "oob" && oobCount == 2 && UIBehavior.gameUI)
         {
             GameObject dtext = GameObject.Find("Level4_Text");
             Debug.Log(dtext);
            Debug.Log("Came inside if");
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Create & Rotate Planks to bridge the gap";
             dtext.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 23;
         }
         else if(type == "oob" && oobCount == 5 && UIBehavior.gameUI)
         {
            GameObject dtext = GameObject.Find("Level4_Text");
            Debug.Log(dtext);
            Debug.Log("Came inside else if");
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Stuck? Remember you can rotate and delete created planks";
            dtext.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 20;
         }
     }
}
