using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level11 : MonoBehaviour, EnemyLevel
{
    public static Level11 Level11Reference;
    public Dictionary<string, string> enemyPlankMap = new Dictionary<string, string>();

    private void Awake()
    {
        enemyPlankMap.Add("Plank (5)", "Enemy (2)");
        enemyPlankMap.Add("Plank (2)", "Enemy (1)");
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


        // Setting the Heater and Plank Btn in the ToolKit Panel to INACTIVE
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);

        // To ensure that the buttons isn't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 120;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 120;

        GlobalVariables.springCap = 2;
        if (Level11Reference == null)
        {
            Level11Reference = this;
        }
        else
        {
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DungeonMaster.dm.setCurrentSceneReference(Level11Reference);
        GlobalVariables.levelScore = 0;
        GameObject scoreText = GameObject.Find("Score_Text");
        if (scoreText != null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("-------------12----------------");
        Debug.Log(GlobalVariables.plankCounter);
        Debug.Log(GlobalVariables.heaterCounter);
        Debug.Log("-----------------------------");
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
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
