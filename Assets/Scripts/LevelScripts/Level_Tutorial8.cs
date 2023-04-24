using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level_Tutorial8 : MonoBehaviour
{
    float timeLeft = 3.0f;
    public GameObject starttxt;
    GameObject levelmode;
    GameObject mainmenumode;
    GameObject initialtext;
    GameObject highlightedArea;
    GameObject highlightedArea1;
    GameObject normalBall;
    GameObject steelBall;
    GameObject woodBall;
    GameObject waterBody;

    void Awake()
    {
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
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(true);

        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);

        // backgroundImage = GameObject.Find("Black_Background");
        initialtext = GameObject.Find("Initial_Text");
        normalBall = GameObject.Find("Normal");
        steelBall = GameObject.Find("Steel");
        woodBall = GameObject.Find("Wood");
        waterBody = GameObject.FindGameObjectWithTag("WaterBody");
        highlightedArea = GameObject.Find("Highlighted area");
        highlightedArea1 = GameObject.Find("Highlighted area (1)");
        waterBody.SetActive(true);
        steelBall.SetActive(true);
        woodBall.SetActive(true);
        normalBall.SetActive(true);
    }
    void Start()
    {
        GlobalVariables.levelScore = 0;
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
        if (!DungeonMaster.dm.simulationMode)
        {
            initialtext.SetActive(true);
            steelBall.SetActive(true);
            woodBall.SetActive(true);
            normalBall.SetActive(true);
            
            var col = highlightedArea.GetComponent<Image>().color;
            col.a = 1f;
            highlightedArea.GetComponent<Image>().color = col;

            var col2 = highlightedArea1.GetComponent<Image>().color;
            col2.a = 1f;
            highlightedArea1.GetComponent<Image>().color = col2;
        }
        else
        {
            steelBall.SetActive(false);
            woodBall.SetActive(false);
            normalBall.SetActive(false);
            initialtext.SetActive(false);
            var col = highlightedArea.GetComponent<Image>().color;
            col.a = 0;
            highlightedArea.GetComponent<Image>().color = col;

            var col2 = highlightedArea1.GetComponent<Image>().color;
            col2.a = 0;
            highlightedArea1.GetComponent<Image>().color = col2;
            
            timeLeft -= Time.deltaTime;
            GameObject[] ball = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject b in ball)
            {
                // check the material of the ball .. if it is steel then change the mass to 15f
                // Debug.Log("ball name"+ b.name);
                if (b.GetComponent<Rigidbody2D>().mass == 5f)
                {
                    b.GetComponent<Rigidbody2D>().mass = 10f;
                }
            }
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
