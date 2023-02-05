using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{ 
    //Outlets
    public StateReference.temperature plankState;
    private SpriteRenderer plankDisplay;
    public bool editable;

    // Start is called before the first frame update
    void Start()
    {
        //Based on its current temperatue, set the color
        plankDisplay = GetComponent<SpriteRenderer>();
        switch (plankState)
        {
            case StateReference.temperature.neutral:
                plankDisplay.material.color = Color.green;
                break;
            case StateReference.temperature.cold:
                plankDisplay.material.color = Color.cyan;
                break;
            case StateReference.temperature.hot:
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
