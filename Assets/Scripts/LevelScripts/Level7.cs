using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level7 : MonoBehaviour, EnemyLevel
{
    public static Level7 Level7Reference;
    public Dictionary<string, string> enemyPlankMap = new Dictionary<string, string>();

    private void Awake()
    {
        enemyPlankMap.Add("Plank (2)", "Enemy (1)");
        enemyPlankMap.Add("Plank (5)", "Enemy (2)");
        // GameObject enemyPlank1 = GameObject.Find("Plank (2)");
        // GameObject enemyPlank2 = GameObject.Find("Plank (5)");
        // GameObject enemy1 = GameObject.Find("Enemy (1)").transform.GetChild(0).gameObject;
        // GameObject enemy2 = GameObject.Find("Enemy (2)").transform.GetChild(0).gameObject;
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
        
        // Set the tool kit panel, operation panel and reset buttons to inactive
        UIBehavior.gameUI.toolKitPanel.SetActive(false);
        // UIBehavior.gameUI.operationPanel.SetActive(false);
        UIBehavior.gameUI.controlPanel.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);


        if (Level7Reference == null)
        {
            Level7Reference = this;
        }
        else
        {
        }
        if (GlobalVariables.kbeCounter == 0)
        {
            GameObject dtext = GameObject.Find("Level7_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: Right Click to Attack Enemies when the ball is moving";
        }
        if (GlobalVariables.kbeCounter > 0 && GlobalVariables.kbeCounter < 2)
        {
            GameObject dtext = GameObject.Find("Level7_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Hint: A Sword always appears from right";
        }
        if (GlobalVariables.kbeCounter  == 2)
        {
            GameObject dtext = GameObject.Find("Level7_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Note: you only have 2 lives";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DungeonMaster.dm.setCurrentSceneReference(Level7Reference);
        GlobalVariables.levelScore = 0;
        GameObject scoreText = GameObject.Find("Score_Text");
        if (scoreText != null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

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
