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
    public GameObject winScreen;

    //Buttons
    private Button[] toolKitButtons;

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
    }

    private void Start()
    {
        DungeonMaster.dm.StartSim += startSim;
        DungeonMaster.dm.StopSim += stopSim;
    }

    public void displayWinScreen()
    {
        var WinSc = Instantiate(winScreen,gameUI.transform.position,Quaternion.identity);
        WinSc.transform.parent = transform;
        //WinSc.transform.SetParent(gameUI.transform);
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
}

//heaterButtonReference.GetComponent<Button>().interactable = true;
//plankButtonReference.GetComponent<Button>().interactable = true;
//springButtonReference.GetComponent<Button>().interactable = true;
