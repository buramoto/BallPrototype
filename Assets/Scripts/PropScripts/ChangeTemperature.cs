using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTemperature : MonoBehaviour
{
    /*
     * This script handles the properties of the heating/cooling element.
     * Its type can be specified in the Unity UI or dynamically with a script
    */
    public bool editable;
    public StateReference.temperature setting;

    private SpriteRenderer elementDisplay;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        elementDisplay = GetComponent<SpriteRenderer>();
        switch (setting)
        {
            case StateReference.temperature.neutral:
                elementDisplay.material.color = Color.green;
                break;
            case StateReference.temperature.hot:
                elementDisplay.material.color = Color.red;
                break;
            case StateReference.temperature.cold:
                elementDisplay.material.color = Color.cyan;
                break;
        }
    }

    public void ChangeTemp(StateReference.temperature temp)
    {
        elementDisplay = GetComponent<SpriteRenderer>();
        setting = temp;
        switch (setting)
        {
            case StateReference.temperature.neutral:
                elementDisplay.material.color = Color.green;
                break;
            case StateReference.temperature.hot:
                elementDisplay.material.color = Color.red;
                break;
            case StateReference.temperature.cold:
                elementDisplay.material.color = Color.cyan;
                break;
        }
    }
}
