using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour
{
    public StateReference.goalColor goalColor;
    // Start is called before the first frame update
    void Start()
    {
        //Initalize goal block with correct color
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (goalColor)
        {
            case StateReference.goalColor.purple:
                renderer.color = Color.red;//CHANGE THIS TO PURPLE
                break;
            case StateReference.goalColor.blue:
                renderer.color = Color.blue;
                break;
            case StateReference.goalColor.yellow:
                renderer.color = Color.yellow;
                break;
            default:
                Debug.Log("Goal color is not set!");
                break;
        }
    }
}
