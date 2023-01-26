using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{ 
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
        
    }
}
