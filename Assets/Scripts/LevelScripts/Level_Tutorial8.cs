using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level_Tutorial8 : MonoBehaviour
{
    float timeLeft = 2.0f;
    public GameObject starttxt;
    GameObject levelmode;
    GameObject mainmenumode;
    GameObject backgroundImage;
    GameObject videoRenderer;
    GameObject playText;
    GameObject initialtext;
    GameObject highlightedArea;
    GameObject highlightedArea1;
    GameObject normalBall;
    GameObject steelBall;
    GameObject woodBall;
    GameObject waterBody;
    bool videoover = false;
    // Start is called before the first frame update

    void Awake()
    {
        //Assigning references for the canvas gameobjects
        levelmode = UIBehavior.gameUI.gameObject.transform.Find("LevelMode").gameObject;
        mainmenumode = UIBehavior.gameUI.gameObject.transform.Find("Main Menu").gameObject;
        // backgroundImage = GameObject.Find("Black_Background");
        videoRenderer = GameObject.Find("Screen");
        playText = GameObject.Find("Play_Text");
        initialtext = GameObject.Find("Initial_Text");
        initialtext.SetActive(false);
        highlightedArea = GameObject.Find("Highlighted area");
        var col = highlightedArea.GetComponent<Image>().color;
        col.a = 0;
        highlightedArea.GetComponent<Image>().color = col;

        highlightedArea = GameObject.Find("Highlighted area (1)");
        var col2 = highlightedArea.GetComponent<Image>().color;
        col2.a = 0;
        highlightedArea.GetComponent<Image>().color = col2;
        backgroundImage = GameObject.Find("Black_Background");
        normalBall = GameObject.Find("Normal");
        steelBall = GameObject.Find("Steel");
        woodBall = GameObject.Find("Wood");
        waterBody = GameObject.FindGameObjectWithTag("WaterBody");
        waterBody.SetActive(false);
        steelBall.SetActive(false);
        woodBall.SetActive(false);
        normalBall.SetActive(false);
        videoover = false;
        levelmode.SetActive(false);
        mainmenumode.SetActive(false);
    }
    void Start()
    {
        GlobalVariables.levelScore = 0;
        GameObject.FindGameObjectsWithTag("MenuBtn")[0].SetActive(true);
        UIBehavior.gameUI.timer.SetActive(true);
        Time.timeScale = 1;
        // Setting all ToolKit & Operation & Control PANEL Btns to ACTIVE
        UIBehavior.gameUI.timer.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        UIBehavior.gameUI.timer.SetActive(false);
        // UIBehavior.gameUI.toolKitPanel.SetActive(false);
        // Setting Plank, Spring & Heater Tools InActive
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);


        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);
        //initialtext = GameObject.Find("Initial_Text");
        //initialtext.SetActive(false);
        levelmode.SetActive(false);
        mainmenumode.SetActive(false);
        steelBall.SetActive(false);
        woodBall.SetActive(false);
        normalBall.SetActive(false);
        waterBody.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = null;
        if (button != null)
        {
            buttonText = button.GetComponentInChildren<TMP_Text>();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            backgroundImage.SetActive(false);
            videoRenderer.SetActive(false);
            playText.SetActive(false);
            levelmode.SetActive(true);
            mainmenumode.SetActive(true);
            // starttext.SetActive(true);
            videoover = true;
            waterBody.SetActive(true);
        }
        if (!DungeonMaster.dm.simulationMode && videoover)
        {
            initialtext.SetActive(true);
            steelBall.SetActive(true);
            woodBall.SetActive(true);
            normalBall.SetActive(true);
            highlightedArea = GameObject.Find("Highlighted area");
            var col = highlightedArea.GetComponent<Image>().color;
            col.a = 1f;
            highlightedArea.GetComponent<Image>().color = col;

            highlightedArea = GameObject.Find("Highlighted area (1)");
            var col2 = highlightedArea.GetComponent<Image>().color;
            col2.a = 1f;
            highlightedArea.GetComponent<Image>().color = col2;
        }
        else if(videoover)
        {
            steelBall.SetActive(false);
            woodBall.SetActive(false);
            normalBall.SetActive(false);
            highlightedArea = GameObject.Find("Highlighted area");
            var col = highlightedArea.GetComponent<Image>().color;
            col.a = 0;
            highlightedArea.GetComponent<Image>().color = col;

            highlightedArea = GameObject.Find("Highlighted area (1)");
            var col2 = highlightedArea.GetComponent<Image>().color;
            col2.a = 0;
            highlightedArea.GetComponent<Image>().color = col2;
            initialtext.SetActive(false);
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0.0f)
            {
                Debug.Log("next level is "+DungeonMaster.dm.nextSceneName);
                GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag("Plank");
                foreach (GameObject objectToDisable in objectsToDisable)
                {
                    objectToDisable.SetActive(false);
                }
                waterBody.SetActive(false);
            }
        }
    }
}
