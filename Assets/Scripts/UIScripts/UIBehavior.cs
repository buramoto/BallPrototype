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

    //Timer
    public GameObject timer;

    //Buttons
    private Button[] toolKitButtons = null;
    private Button[] operationButtons = null;

    //Elements
    //IDictionary<string, GameObject> modeElements = new Dictionary<string, GameObject>();//All elements for each mode
    public GameObject levelMode;
    public GameObject mainMenuMode;
    public GameObject levelSelectPanel;
    public GameObject buttonPrefab;
    public GameObject toolKitPanel;
    public GameObject operationPanel;
    public GameObject controlPanel;

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
            operationButtons = operationPanel.GetComponentsInChildren<Button>();
            timer = GameObject.Find("Timer");
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
    }

    private void initalizeLevel(Scene scene, LoadSceneMode mode)
    {
        if(activeScreen != null)
        {
            Destroy(activeScreen);
        }
        //if/else statement below sets the UI elements to either level or main menu mode
        if (scene.name == "MainMenu")
        {
            levelMode.SetActive(false);
            mainMenuMode.SetActive(true);
            initMainMenu();
        }
        else
        {
            mainMenuMode.SetActive(false);
            levelMode.SetActive(true);

        }
    }

    public void updateTimer(float minutes, float seconds) {
        if(SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2") {
            return;
        }
        TextMeshProUGUI tm =  timer.GetComponent<TextMeshProUGUI>(); 
        tm.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Winscreen Func
    public void displayWinScreen()
    {
        activeScreen = Instantiate(winScreen, gameObject.transform);
    }

    // GameOver Func
    public void displayGameOverScreen()
    {
        activeScreen = Instantiate(gameOverScreen, gameObject.transform);
    }


    public void displayNextLevelScreen(string sceneName)
    {
        activeScreen = Instantiate(nextLevelScreen, gameObject.transform);
    }

    private void initMainMenu()
    {
        Debug.LogWarning("Initalizing main menu");

        foreach(string fileName in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Scenes"), "*.unity"))
        {
            string sceneName = Path.GetFileNameWithoutExtension(fileName);
            if (sceneName == "MainMenu")
            {
                continue;
            }
            GameObject button = Instantiate(buttonPrefab, levelSelectPanel.transform);
            button.GetComponentInChildren<TMP_Text>().text = sceneName;
            button.GetComponentInChildren<Button>().onClick.AddListener(delegate { DungeonMaster.dm.loadNextLevel(sceneName); });
        }
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
        setOperationInactive();
        disableResetButton();
        // Debug.Log("UI Behavior -- HighlightedObject"+DungeonMaster.dm.highlightedObject);
    }
    private void stopSim(StateReference.resetType type)
    {
        //Debug.Log("UI stopping sim");
        for (int i = 0; i < toolKitButtons.Length; i++)
        {
            toolKitButtons[i].interactable = true;
        }
        enableResetButton();
        //Future scope: create indication for stop type. E.g. arrow pointing to oob coords when type is oob
    }

    public void setOperationInactive(){
        for(int i=0; i < operationButtons.Length; i++)
        {
            operationButtons[i].interactable = false;
        }
    }
    
    public void setOperationActive(GameObject toolkitInstance){
        for(int i=0; i < operationButtons.Length; i++)
        {
            if(toolkitInstance.CompareTag("TempChange")){ // if heater is selected then only DELETE BUTTON is INTERACTABLE
                if(operationButtons[i].CompareTag("DeleteButton")){
                    operationButtons[i].interactable = true;
                }
                else{
                    operationButtons[i].interactable = false;
                }
            }
            else{
                operationButtons[i].interactable = true;
            }
        }
    }

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

    


}
