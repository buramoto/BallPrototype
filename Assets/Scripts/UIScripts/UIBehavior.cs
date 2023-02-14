using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script used to make sure the canvas does not destroy itself when switching levels
/// 
/// Future scope: This script can also be used to transition between menus and gameplay
/// </summary>
public class UIBehavior : MonoBehaviour
{
    public static UIBehavior gameUI;

    //Menus
    public GameObject winScreen ;

    //Buttons
    private Button[] toolKitButtons;
    private Button[] operationButtons;

    //Reset variables
    public Vector3 oobCoords; //Future scope: place an arrow where ball went oob

    void Awake()
    {
        if(gameUI == null)
        {
            DontDestroyOnLoad(gameObject);
            gameUI = this;
        }
        else
        {
            Destroy(gameObject);
        }
        toolKitButtons = gameObject.transform.Find("Toolkit").GetComponentsInChildren<Button>();
        operationButtons = gameObject.transform.Find("Operations").GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        DungeonMaster.dm.StartSim += startSim;
        DungeonMaster.dm.StopSim += stopSim;
    }

    public void displayWinScreen()
    {
        Instantiate(winScreen, gameObject.transform);
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
        // Debug.Log("UI Behavior -- HighlightedObject"+DungeonMaster.dm.highlightedObject);
    }
    private void stopSim(StateReference.resetType type)
    {
        //Debug.Log("UI stopping sim");
        for (int i = 0; i < toolKitButtons.Length; i++)
        {
            toolKitButtons[i].interactable = true;
        }
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


}

//heaterButtonReference.GetComponent<Button>().interactable = true;
//plankButtonReference.GetComponent<Button>().interactable = true;
//springButtonReference.GetComponent<Button>().interactable = true;
