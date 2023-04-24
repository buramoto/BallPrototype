using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

/// <summary>
/// This script used to make sure the canvas does not destroy itself when switching levels
/// 
/// Future scope: This script can also be used to transition between menus and gameplay
/// </summary>
public class UIBehavior : MonoBehaviour
{
    public static UIBehavior gameUI;

    //Menus
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject nextLevelScreen;
    public GameObject RedSplashScreen;


    //Timer
    public GameObject timer;

    //Buttons
    private Button[] toolKitButtons = null;
    // private Button[] operationButtons = null;

    //Health Slider
    Slider _healthSlider;

    //Elements
    //IDictionary<string, GameObject> modeElements = new Dictionary<string, GameObject>();//All elements for each mode
    public GameObject levelMode;
    public GameObject mainMenuMode;
    public GameObject levelSelectPanel;
    public GameObject buttonPrefab;
    public GameObject toolKitPanel;
    public GameObject operationPanel;
    public GameObject controlPanel;
    public GameObject mainMenuBtn;
    public GameObject plankCount;
    public GameObject springCount;
    public GameObject heaterCount;
    public GameObject mainLevelButtons;
    public GameObject bombLevelButtons;
    public GameObject tutorialLevelButtons;

    //Tooltip
    private GameObject activeTooltip;
    public Vector3 offset;

    //Reset variables
    public Vector3 oobCoords; //Future scope: place an arrow where ball went oob

    private GameObject activeScreen = null;

    void Awake()
    {
        if(gameUI == null)
        {
            DontDestroyOnLoad(gameObject);
            gameUI = this;
            toolKitButtons = toolKitPanel.GetComponentsInChildren<Button>();
            plankCount = toolKitPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(x => x.name == "PlankCount").First().gameObject;
            springCount = toolKitPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(x => x.name == "SpringCount").First().gameObject;
            heaterCount = toolKitPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(x => x.name == "HeaterCount").First().gameObject;
            // plankCount = GameObject.FindGameObjectsWithTag("PlankCount")[0];
            // springCount = GameObject.FindGameObjectsWithTag("SpringCount")[0];
            // heaterCount = GameObject.FindGameObjectsWithTag("HeaterCount")[0];
            // operationButtons = operationPanel.GetComponentsInChildren<Button>();
            mainMenuBtn = GameObject.FindGameObjectsWithTag("MenuBtn")[0];
            timer = GameObject.Find("Timer");
            _healthSlider = GameObject.FindGameObjectsWithTag("HealthSlider")[0].GetComponent<Slider>();
            SceneManager.sceneLoaded += initalizeLevel;
            /*
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                toolKitButtons = transform.Find("Toolkit").GetComponentsInChildren<Button>();
                operationButtons = transform.Find("Operations").GetComponentsInChildren<Button>();
            }
            */
        }
        else
        {
            /*
            if (gameUI.toolKitButtons == null)
            {
                gameUI.toolKitButtons = gameUI.transform.Find("Toolkit").GetComponentsInChildren<Button>();
                gameUI.operationButtons = gameUI.transform.Find("Operations").GetComponentsInChildren<Button>();
            }
            */

            Destroy(gameObject);
        }
        //toolKitButtons = transform.Find("Toolkit").GetComponentsInChildren<Button>();
        //operationButtons = transform.Find("Operations").GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        DungeonMaster.dm.StartSim += startSim;
        DungeonMaster.dm.StopSim += stopSim;
        //Debug.Log('');
        //RedSplashScreen = GameObject.FindWithTag("HealthLoss");
        RedSplashScreen = Instantiate(RedSplashScreen);
        DontDestroyOnLoad(RedSplashScreen);
        RedSplashScreen = RedSplashScreen.transform.GetChild(0).gameObject;
        //Debug.Log("Name of RedSplashScreen: "+RedSplashScreen.name);

        //RedSplashScreen = GameObject.FindWithTag("HealthLoss");
    }

    public void changeButtonStateToStart() {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Start";
    }

    public void changeButtonColor(bool mode) {
        if(mode) {
            controlPanel.GetComponentsInChildren<Button>(true)[0].GetComponentsInChildren<Image>(true)[0].color = new Color32(231,64,64,255);
        } else {
            controlPanel.GetComponentsInChildren<Button>(true)[0].GetComponentsInChildren<Image>(true)[0].color = new Color32(52,195,52,255);
        }
    }

    private void initalizeLevel(Scene scene, LoadSceneMode mode)
    {
        if(activeScreen != null)
        {
            Destroy(activeScreen);
        }
        //if/else statement below sets the UI elements to either level or main menu mode
        if (scene.name == "MainMenu" || scene.name == "New_MainMenu")
        {
            mainMenuBtn.SetActive(false);
            levelMode.SetActive(false);
            mainMenuMode.SetActive(true);
            initMainMenu();
        }
        else
        {
            mainMenuBtn.SetActive(true);
            mainMenuMode.SetActive(false);
            levelMode.SetActive(true);
            setMaxHealth(DungeonMaster.dm._ballHealth.MaxHealth);
        }
    }

    private void Update()
    {
        if (RedSplashScreen != null)
        {
            if (RedSplashScreen.GetComponent<Image>().color.a > 0)
            {
                var color = RedSplashScreen.GetComponent<Image>().color;
                color.a -= 0.005f;
                RedSplashScreen.GetComponent<Image>().color = color;
            }

        }
    }

    public void setMaxHealth(int maxHealth) {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth;
    }

    public void setHealth(int health) {
        _healthSlider.value = health;
    }

    public void setRedSplashScreen()
    {
        Debug.Log("Inside Red Splash Screen Func");
        var color = RedSplashScreen.GetComponent<Image>().color;
        color.a = 1.0f;
        RedSplashScreen.GetComponent<Image>().color = color;
        Debug.Log("Value of alpha" + RedSplashScreen.GetComponent<Image>());
        Debug.Log("Value of alpha" + RedSplashScreen.GetComponent<Image>().color.a);
    }

    public void updateTimer(float minutes, float seconds) {
        if(SceneManager.GetActiveScene().name == "New_MainMenu" || SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2") {
            return;
        }
        // TextMeshProUGUI tm =  timer.GetComponent<TextMeshProUGUI>(); 
        // tm.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Winscreen Func
    public void displayWinScreen()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Start";
        changeButtonColor(false);
        activeScreen = Instantiate(winScreen, gameObject.transform);
        GameObject FinalScore = GameObject.Find("Final_Score");
        Debug.Log("This is the score go" + FinalScore);
        if (FinalScore != null)
        {
            FinalScore.GetComponent<TMPro.TextMeshProUGUI>().text = "Your Score: " + GlobalVariables.levelScore.ToString();
        }
    }

    // GameOver Func
    public void displayGameOverScreen()
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Start";
        changeButtonColor(false);
        activeScreen = Instantiate(gameOverScreen, gameObject.transform);
    }


    public void displayNextLevelScreen(string sceneName)
    {
        GameObject button = GameObject.Find("StartButton");
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Start";
        changeButtonColor(false);
        activeScreen = Instantiate(nextLevelScreen, gameObject.transform);
        GameObject FinalScore = GameObject.Find("Final_Score");
        Debug.Log("This is the score go" + FinalScore);
        if (FinalScore != null)
        {
            FinalScore.GetComponent<TMPro.TextMeshProUGUI>().text = "Your Score: " + GlobalVariables.levelScore.ToString();
        }
    }

class MyComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        // Check if both strings have "Level" in them
        x = Path.GetFileNameWithoutExtension(x); 
        y = Path.GetFileNameWithoutExtension(y); 
        if (x.Contains("Level") && y.Contains("Level"))
        {
            // Extract the number after "Level" and convert it to integer
            int xLevel = int.Parse(x.Substring(5));
            int yLevel = int.Parse(y.Substring(5));

            // Compare the levels
            return xLevel.CompareTo(yLevel);
        }
        else
        {
            // If one or both strings don't have "Level" in them, use default comparer
            return string.Compare(x, y);
        }
    }
}

    private void initMainMenu()
    {
        Debug.LogWarning("Initalizing main menu");
        var tutorialLevelBtns = tutorialLevelButtons.GetComponentsInChildren<Button>(true);
        var mainLevelBtns = mainLevelButtons.GetComponentsInChildren<Button>(true);
        var bombLevelBtns = bombLevelButtons.GetComponentsInChildren<Button>(true);
        for(int i=0;i<tutorialLevelBtns.Length;i++) {
            if(DungeonMaster.completedTutorialScenes[i]) {
                tutorialLevelBtns[i].GetComponentInChildren<Image>().color = new Color32(52,195,52,255);
            } else if (DungeonMaster.attemptedTutorialScenes[i]) {
                tutorialLevelBtns[i].GetComponentInChildren<Image>().color = new Color32(231,64,64,255);
            }
        }
        for(int i=0;i<mainLevelBtns.Length;i++) {
            if(DungeonMaster.completedMainScenes[i]) {
                mainLevelBtns[i].GetComponentInChildren<Image>().color = new Color32(52,195,52,255);
            } else if (DungeonMaster.attemptedMainScenes[i]) {
                mainLevelBtns[i].GetComponentInChildren<Image>().color = new Color32(231,64,64,255);
            }
        }
        for(int i=0;i<bombLevelBtns.Length;i++) {
            if(DungeonMaster.completedBombScenes[i]) {
                bombLevelBtns[i].GetComponentInChildren<Image>().color = new Color32(52,195,52,255);
            } else if (DungeonMaster.attemptedBombScenes[i]) {
                bombLevelBtns[i].GetComponentInChildren<Image>().color = new Color32(231,64,64,255);
            }
        }


        // if(levelSelectPanel.GetComponentsInChildren<Button>(true).Length > 1) {
        //     var buttons = levelSelectPanel.GetComponentsInChildren<Button>(true);
        //     for(var i=0;i<buttons.Length;i++) {
        //         var buttonText = buttons[i].GetComponentInChildren<TMP_Text>().text;
        //         if(DungeonMaster.completedLevelNumbers.Contains(buttonText)) {
        //             buttons[i].GetComponentInChildren<Image>().color = new Color32(52,195,52,255);
        //         } else if (DungeonMaster.attemptedLevelNumbers.Contains(buttonText)) {
        //             buttons[i].GetComponentInChildren<Image>().color = new Color32(231,64,64,255);
        //         }
        //     }
        // } else {
        //     // string[] fileNames = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Scenes"), "*.unity");
        //     // for(var i=0;i<fileNames.Length;i++)
        //     // {
        //     //     fileNames[i] = Path.GetFileNameWithoutExtension(fileNames[i]);
        //     // }
        //     // fileNames = fileNames.Where(fileName => fileName != "MainMenu").ToArray();
        //     // Array.Sort(fileNames, new MyComparer());
        //     string[] fileNames = DungeonMaster.scenes;
        //     foreach(string fileName in fileNames)
        //     {
        //         GameObject button = Instantiate(buttonPrefab, levelSelectPanel.transform);
        //         button.GetComponentInChildren<TMP_Text>().text = fileName.Substring(5);
        //         // Debug.Log("CHECK CONTAINMENT: " + DungeonMaster.levelsCompleted.Contains(fileName));
        //         button.GetComponentInChildren<Button>().onClick.AddListener(delegate { DungeonMaster.dm.loadNextLevel(fileName); });
        //     }
        // }
    }

    private void levelSelect(string levelName)
    {
        Debug.Log("Loading scene " + levelName);
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Event handlers for stop and start sim
    /// </summary>
    /// This should be changed to an event
    private void startSim()
    {
        //Debug.Log("UI starting sim");
        for(int i=0; i < toolKitButtons.Length; i++)
        {
            toolKitButtons[i].interactable = false;
        }
        // setOperationInactive();
        disableResetButton();
        UIBehavior.gameUI.changeButtonColor(true);
        // Debug.Log("UI Behavior -- HighlightedObject"+DungeonMaster.dm.highlightedObject);
    }
    private void stopSim(StateReference.resetType type)
    {
        //Debug.Log("UI stopping sim");
        resetAirflow();
        for (int i = 0; i < toolKitButtons.Length; i++)
        {
            toolKitButtons[i].interactable = true; 
        }
        enableResetButton();
        UIBehavior.gameUI.changeButtonColor(false);
        //Future scope: create indication for stop type. E.g. arrow pointing to oob coords when type is oob
    }

    // public void setOperationInactive(){
    //     for(int i=0; i < operationButtons.Length; i++)
    //     {
    //         operationButtons[i].interactable = false;
    //     }
    // }
    
    // public void setOperationActive(GameObject toolkitInstance){
    //     for(int i=0; i < operationButtons.Length; i++)
    //     {
    //         if(toolkitInstance.CompareTag("TempChange")){ // if heater is selected then only DELETE BUTTON is INTERACTABLE
    //             if(operationButtons[i].CompareTag("DeleteButton")){
    //                 operationButtons[i].interactable = true;
    //             }
    //             else{
    //                 operationButtons[i].interactable = false;
    //             }
    //         }
    //         else{
    //             operationButtons[i].interactable = true;
    //         }
    //     }
    // }

    // used to disable RESET button when simulation mode is ON
    public void disableResetButton()
    {
        controlPanel.GetComponentsInChildren<Button>(true)[2].interactable = false;
    }

    // used to enable RESET Button when simulation mode is OFF
    public void enableResetButton()
    {
        controlPanel.GetComponentsInChildren<Button>(true)[2].interactable = true;
    }

    /// <summary>
    /// This method to be used by toolkit buttons to trigger a tooltip for the buttons
    /// </summary>
    /// <param name="tip"></param>
    public void displayToolTip(GameObject button)
    {
        if (!button.GetComponent<Button>().IsInteractable())
        {
            return;
        }
        activeTooltip = button;
        foreach(Transform child in button.transform)
        {
            if(child.tag == "Tooltip")
            {
                //child.GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
                child.gameObject.SetActive(true);
            }
            else
            {
                //child.GetComponent<TextMeshProUGUI>().enabled = false;
                child.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Removes the currently active tooltip
    /// </summary>
    public void removeToolTip()
    {
        if (activeTooltip)
        {
            foreach (Transform child in activeTooltip.transform)
            {
                if (child.tag == "Tooltip")
                {
                    //child.GetComponent<CanvasRenderer>().enabled = false;
                    child.gameObject.SetActive(false);
                }
                else
                {
                    //child.GetComponent<CanvasRenderer>().enabled = true;
                    child.gameObject.SetActive(true);
                }
            }
        }
    }


    public void resetAirflow()
    {
        if (GameObject.FindWithTag("Airflow") != null)
        {
            GameObject.FindWithTag("Airflow").GetComponent<AreaEffector2D>().forceMagnitude = 15.0f;

            ParticleSystem particleSystem = GameObject.FindWithTag("WindParticles").GetComponent<ParticleSystem>();
            ParticleSystem.VelocityOverLifetimeModule velocityModule = particleSystem.velocityOverLifetime;
            ParticleSystem.MinMaxCurve speed = velocityModule.speedModifier;
            float newSpeed = 0.4f;
            speed = new ParticleSystem.MinMaxCurve(newSpeed);
            velocityModule.speedModifier = speed;
        }
    }


}
