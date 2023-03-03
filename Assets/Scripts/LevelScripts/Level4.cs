using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level4 : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        UIBehavior.gameUI.toolKitPanel.SetActive(true);
        UIBehavior.gameUI.operationPanel.SetActive(true);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);

        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
        
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        // To ensure that the buttons isn't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 100;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 100;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void OutOfBounds(string type)
    // {
    //     // Increment the counter for out of bounds
    //     oobCount++;

    //     // If the counter is 2, then display the text to assist the player
    //     if (type == "oob" && oobCount == 2 && UIBehavior.gameUI)
    //     {
    //         GameObject dtext = GameObject.Find("Spring_Text");
    //         Debug.Log(dtext);
    //         dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Stuck? Place spring next to the second plank facing the ball";
    //         dtext.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 23;
    //     }
    // }
}
