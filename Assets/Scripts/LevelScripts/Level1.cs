using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level1 : MonoBehaviour
{
    public GameObject image;
    // Start is called before the first frame update

    private void Awake()
    {

    }

    void Start()
    {
        GlobalVariables.levelScore = 0;
        GameObject scoreText = GameObject.Find("Score_Text");
        if (scoreText != null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }
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
        UIBehavior.gameUI.toolKitPanel.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);
        image = GameObject.Find("Arrow");
        var col = image.GetComponent<Image>().color;
        col.a = 0.3f;
        image.GetComponent<Image>().color = col;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if(!DungeonMaster.dm.simulationMode)
        {
            var col = image.GetComponent<Image>().color;
            col.a = 0.3f;
            image.GetComponent<Image>().color = col;
        }
        else
        {
            var col = image.GetComponent<Image>().color;
            col.a = 0;
            image.GetComponent<Image>().color = col;
        }
    }
}
