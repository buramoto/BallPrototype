using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static_Plank : MonoBehaviour
{
    // Start is called before the first frame update
    public StateReference.temperature plankState;
    private SpriteRenderer plankDisplay;
    Collider2D _collider;

    void Start()
    {
        // variables needed to move PLANK 
        _collider = GetComponent<Collider2D>();

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
    }
}
