using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        Time.timeScale = 1;
        // GameObject.Find("Timer").SetActive(false);
        UIBehavior.gameUI.timer.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.SetActive(false);
        UIBehavior.gameUI.operationPanel.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
