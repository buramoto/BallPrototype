using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level15 : MonoBehaviour, EnemyLevel
{
    public static Level15 Level15Reference;
    public Dictionary<string, string> enemyPlankMap = new Dictionary<string, string>();
    GameObject levelmode;
    GameObject mainmenumode;
    GameObject backgroundImage;
    GameObject videoRenderer;
    GameObject playText;

    private void Awake()
    {
         enemyPlankMap.Add("Plank (6)", "Enemy (3)");
        enemyPlankMap.Add("Plank (2)", "Enemy");
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


        // Setting the Spring Btn, WOOD Material in the ToolKit Panel to INACTIVE
        UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(false);
        //UIBehavior.gameUI.toolKitPanel.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(false);

        // To ensure that the buttons isn't stretched
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
        UIBehavior.gameUI.toolKitPanel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;

        GlobalVariables.springCap = 4;
        GlobalVariables.heaterCap = 2;

        //Assigning references for the canvas gameobjects
        levelmode = UIBehavior.gameUI.gameObject.transform.Find("LevelMode").gameObject;
        mainmenumode = UIBehavior.gameUI.gameObject.transform.Find("Main Menu").gameObject;
        backgroundImage = GameObject.Find("Black_Background");
        videoRenderer = GameObject.Find("Screen");
        playText = GameObject.Find("Play_Text");

        if (Level15Reference == null)
        {
            Level15Reference = this;
        }
        else
        {
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        DungeonMaster.dm.setCurrentSceneReference(Level15Reference);
        GlobalVariables.levelScore = 0;
        GameObject scoreText = GameObject.Find("Score_Text");
        if (scoreText != null)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
        }
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();

        levelmode.SetActive(false);
        mainmenumode.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            backgroundImage.SetActive(false);
            videoRenderer.SetActive(false);
            playText.SetActive(false);
            levelmode.SetActive(true);
            mainmenumode.SetActive(true);
        }
        UIBehavior.gameUI.springCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.springCap.ToString();
        UIBehavior.gameUI.heaterCount.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.heaterCap.ToString();
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
