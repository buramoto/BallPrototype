using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLevel2 : MonoBehaviour, EnemyLevel
{
    public static MainLevel2 MainLevel2Reference;
    public Dictionary<string, string> enemyPlankMap = new Dictionary<string, string>();
    private int oobCount = 0;
    private void Awake()
    {
        enemyPlankMap.Add("Plank (6)", "Enemy (1)");
        GameObject.FindGameObjectsWithTag("MenuBtn")[0].SetActive(true);
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

        // Setting the Heater, STEEL & WOOD MATERIAL of ToolKit Panel to INACTIVE
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(false);

        // To ensure that the buttons isn't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

        GlobalVariables.plankCap = 3;
        GlobalVariables.springCap = 2;

        if (MainLevel2Reference == null)
        {
            MainLevel2Reference = this;
        }
        else
        {
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DungeonMaster.dm.setCurrentSceneReference(MainLevel2Reference);
        // GlobalVariables.levelScore = 0;
        // GameObject scoreText = GameObject.Find("Score_Text");
        // if (scoreText != null)
        // {
        //     scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        // }
        UIBehavior.gameUI.plankCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.plankCap.ToString();
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UIBehavior.gameUI.plankCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.plankCap.ToString();
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
    }

    public void OutOfBounds(string type)
    {
        // Increment the counter for out of bounds
        oobCount++;

        // If the counter is 2, then display the text to assist the player
        if (type == "oob" && oobCount == 2)
        {
            GameObject dtext = GameObject.Find("Level6_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Try aligning Wood Planks & a Spring";
        }
        if (type == "oob" && oobCount == 5)
        {
            GameObject dtext = GameObject.Find("Level6_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Try Rotating the Wood Planks/Spring";
        }
    }

    public void resetEnemies()
    {
        foreach (KeyValuePair<string, string> pair in enemyPlankMap)
        {
            string enemyName = pair.Value;
            GameObject enemy = GameObject.Find(enemyName).transform.GetChild(0).gameObject;
            if(enemy.activeSelf) {
                enemy.SendMessage("resetEnemy");
            }
        }
    }

    public void dropEnemy(string plankName)
    {
        if(enemyPlankMap.ContainsKey(plankName)) {
            GameObject enemy = GameObject.Find(enemyPlankMap[plankName]).transform.GetChild(0).gameObject;
            if(enemy.activeSelf) {
                Debug.Log("ENEMY NAME:" + enemy.name);
                enemy.AddComponent<Rigidbody2D>();
                enemy.GetComponent<Animation>().enabled = false;
                enemy.GetComponent<Collider2D>().enabled = false;
            }
            // rb.useGravity = true;
            // enemy.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
