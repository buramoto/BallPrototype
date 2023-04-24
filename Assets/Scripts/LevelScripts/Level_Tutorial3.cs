using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Tutorial3 : MonoBehaviour
{
    // Start is called before the first frame update
    private int oobCount = 0;
    public GameObject image;
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
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(true);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(true);

        image = GameObject.Find("Plank_Outline");
        var col = image.GetComponent<Image>().color;
        col.a = 0;
        image.GetComponent<Image>().color = col;
        
        // Setting the SPRING, HEATER, STEEL MATERIAL & WOOD MATERIAL of the ToolKit Panel INACTIVE
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(false);

        // To ensure that the buttons aren't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 120;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 120;

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

    public IEnumerator Rotate(Vector3 angles, float duration )
 {
    //  rotating = true ;
    Debug.Log("Inside Rotate");
     Quaternion startRotation = image.transform.rotation ;
     Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
     for( float t = 0 ; t < duration ; t+= Time.deltaTime )
     {
         image.transform.rotation = Quaternion.Lerp( startRotation, endRotation, t / duration );
         yield return null;
     }
     image.transform.rotation = endRotation;
    Debug.Log("Rotation Done");
    //  rotating = false;
 }

    public void OutOfBounds(string type)
    {
        // Increment the counter for out of bounds
        oobCount++;
        Debug.Log("Came inside the function");
         // If the counter is 2, then display the text to assist the player
         if (type == "oob" && oobCount == 2 && UIBehavior.gameUI)
         {
             GameObject dtext = GameObject.Find("Level3_Text");
             Debug.Log(dtext);
            Debug.Log("Came inside if");
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint!";
             dtext.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 23;
             var col = image.GetComponent<Image>().color;
        col.a = 1;
        image.GetComponent<Image>().color = col;
        StartCoroutine(Rotate(new Vector3(0,0,-20), 5f));
         }
         else if(type == "oob" && oobCount == 5 && UIBehavior.gameUI)
         {
            GameObject dtext = GameObject.Find("Level3_Text");
            Debug.Log(dtext);
            Debug.Log("Came inside else if");
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Stuck? Remember you can rotate and delete created planks";
            dtext.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 20;
         }
     }
}
