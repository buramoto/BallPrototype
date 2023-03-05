using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level1 : MonoBehaviour
{
    public GameObject image;
    // Start is called before the first frame update
    void Start()
    {   
        GameObject.FindGameObjectsWithTag("MenuBtn")[0].SetActive(true);
        Time.timeScale = 1;
        // GameObject.Find("Timer").SetActive(false);
        UIBehavior.gameUI.timer.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.SetActive(false);
        UIBehavior.gameUI.operationPanel.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);
        image = GameObject.Find("Arrow");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if(buttonText.text=="Start")
        {
            image.SetActive(true);
        }
        else if(buttonText.text=="Stop")
        {
            image.SetActive(false);
        }
    }
}
