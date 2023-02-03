using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{ 
    /*
     * This script handles the behvaior of the plank. Its temperature can be adjusted both in the editor
     * or via script. Eventually, we should change the colors to be configurable rather than hardcoded
     * i.e. The normal temp should reference some global variable for that color
    */
    public enum plankTemp
    {
        normal,
        ice,
        fire
    }

    //Outlets
    public plankTemp plankState;
    private SpriteRenderer plankDisplay;
    // Start is called before the first frame update
    void Start()
    {
        //Based on its current temperatue, set the color
        plankDisplay = GetComponent<SpriteRenderer>();
        switch (plankState)
        {
            case plankTemp.normal:
                plankDisplay.material.color = Color.green;
                break;
            case plankTemp.ice:
                plankDisplay.material.color = Color.cyan;
                break;
            case plankTemp.fire:
                plankDisplay.material.color = Color.red;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Eventually, we may need to change the temperature of the plank while the level is active.
        //This update should go here
    }
}
