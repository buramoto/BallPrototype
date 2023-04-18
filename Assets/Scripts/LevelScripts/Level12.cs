using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level12 : MonoBehaviour
{
    GameObject levelmode;
    GameObject mainmenumode;
    GameObject backgroundImage;
    GameObject videoRenderer;
    GameObject playText, playText1;
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

        // Setting the Plank Btn in the ToolKit Panel to INACTIVE
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);

        // To ensure that the buttons isn't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

        GlobalVariables.springCap = 2;
        GlobalVariables.heaterCap = 1;

        //Assigning references for the canvas gameobjects
        levelmode = UIBehavior.gameUI.gameObject.transform.Find("LevelMode").gameObject;
        mainmenumode= UIBehavior.gameUI.gameObject.transform.Find("Main Menu").gameObject;
        backgroundImage = GameObject.Find("Black_Background");
        videoRenderer = GameObject.Find("Screen");
        playText = GameObject.Find("Play_Text");
        playText1 = GameObject.Find("Play_Text (1)");
        
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
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();

        levelmode.SetActive(false);
        mainmenumode.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            backgroundImage.SetActive(false);
            videoRenderer.SetActive(false);
            playText.SetActive(false);
            playText1.SetActive(false);
            levelmode.SetActive(true);
            mainmenumode.SetActive(true);
        }
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
    }
}
