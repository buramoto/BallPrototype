using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTemperature : MonoBehaviour
{

    //Settings
    public enum tempType
    {
        normal,
        heater,
        cooler
    }

    public tempType setting;
    private SpriteRenderer elementDisplay;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        elementDisplay = GetComponent<SpriteRenderer>();
        switch (setting)
        {
            case tempType.normal:
                elementDisplay.material.color = Color.green;
                break;
            case tempType.heater:
                elementDisplay.material.color = Color.red;
                break;
            case tempType.cooler:
                elementDisplay.material.color = Color.cyan;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
