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
                //renderer.color = new Color(181, 0, 255);//CHANGE THIS TO PURPLE
                break;
            case StateReference.goalColor.white:
                //renderer.color = Color.white;
                break;
            case StateReference.goalColor.yellow:
                //renderer.color = Color.yellow;
                break;
            case StateReference.goalColor.green:
                // commented this line so that the goal block isnt green in color
                //renderer.color = Color.green;
                break;
            default:
                Debug.Log("Goal color is not set!");
                break;
        }
    }
}
