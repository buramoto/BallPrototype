using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level_Tutorial7 : MonoBehaviour
{
    public float timeLeft = 3.0f;
    public GameObject starttxt;
    // GameObject levelmode;
    GameObject mainmenumode;
    // GameObject backgroundImage;
    // GameObject videoRenderer;
    // GameObject playText;
    GameObject starttext;
    GameObject normalBall;
    GameObject steelBall;
    GameObject woodBall;
    // bool videoover = false;
    // Start is called before the first frame update

    void Awake()
    {
        //Assigning references for the canvas gameobjects
        // levelmode = UIBehavior.gameUI.gameObject.transform.Find("LevelMode").gameObject;
        // mainmenumode = UIBehavior.gameUI.gameObject.transform.Find("Main Menu").gameObject;
        GameObject.FindGameObjectsWithTag("MenuBtn")[0].SetActive(true);
        Time.timeScale = 1;
        // Setting all ToolKit & Operation & Control PANEL Btns to ACTIVE
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);

        UIBehavior.gameUI.timer.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.SetActive(false);

        // backgroundImage = GameObject.Find("Black_Background");
        // videoRenderer = GameObject.Find("Screen");
        // playText = GameObject.Find("Play_Text");
        starttext = GameObject.Find("StartText");
        normalBall = GameObject.Find("Normal");
        steelBall = GameObject.Find("Steel");
        woodBall = GameObject.Find("Wood");
        // videoover = false;
        // levelmode.SetActive(false);
        // mainmenumode.SetActive(false);
    }
    void Start()
    {
        GlobalVariables.levelScore = 0;
        // GameObject.FindGameObjectsWithTag("MenuBtn")[0].SetActive(true);
        // UIBehavior.gameUI.timer.SetActive(true);
        // Time.timeScale = 1;
        // Setting all ToolKit & Operation & Control PANEL Btns to ACTIVE
        // UIBehavior.gameUI.timer.SetActive(true);
        // UIBehavior.gameUI.toolKitPanel.SetActive(true);
        // UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        // UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        // UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        // UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
        // UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        // UIBehavior.gameUI.timer.SetActive(false);
        // UIBehavior.gameUI.toolKitPanel.SetActive(false);
        // UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);
        // starttext = GameObject.Find("StartText");
        // starttext.SetActive(false);
        // levelmode.SetActive(false);
        // mainmenumode.SetActive(false);
        steelBall.SetActive(false);
        woodBall.SetActive(false);
        normalBall.SetActive(false);
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
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     backgroundImage.SetActive(false);
        //     videoRenderer.SetActive(false);
        //     playText.SetActive(false);
        //     levelmode.SetActive(true);
        //     mainmenumode.SetActive(true);
        //     starttext.SetActive(true);
        //     videoover = true;
        // }
        if (!DungeonMaster.dm.simulationMode)
        {
            starttext.SetActive(true);
            steelBall.SetActive(false);
            woodBall.SetActive(false);
            normalBall.SetActive(false);
        }
        else
        {
            steelBall.SetActive(true);
            woodBall.SetActive(true);
            normalBall.SetActive(true);
            starttext.SetActive(false);
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0.0f)
            {
                //Debug.Log("next level is "+DungeonMaster.dm.nextSceneName);
                GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag("Airflow");
                foreach (GameObject objectToDisable in objectsToDisable)
                {
                    objectToDisable.SetActive(false);
                }
            }
        }
    }
}
