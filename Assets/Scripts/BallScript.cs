using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    /*
     * This script handles the behavior of the player's ball. It should handle collusions and interactions
     * such as destroying ice planks while hot
    */
    //State variables

    //Outputs
    public GameObject ball;
    public StateReference.temperature tempState;
    public Vector2 startPosition;

    //Private variables
    private SpriteRenderer ballDisplay;
    private Rigidbody2D ballPhysics;
    

    // Start is called before the first frame update
    void Start()
    {
        //Set the ball to its starting position (This should be changed to be configurable based on level
        ball.transform.position = startPosition;
        //Set inital temperature
        tempState = StateReference.temperature.neutral;
        ball.SetActive(true);
        ballDisplay = GetComponent<SpriteRenderer>();
        ballDisplay.material.color = Color.gray;
        ballPhysics = GetComponent<Rigidbody2D>();
        stopSim();
    }

    //When colliding with an object, invoke appropriate function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Plank":
                plankCollision(collision.gameObject);
                break;
            case "Static_Plank":
                staticplankCollision(collision.gameObject);
                break;
        }
    }

    //Change temperature based on heating/cooling element
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Checkpoint":
                checkpointCollision(other.gameObject);
                break;
            case "TempChange":
                elementCollision(other.gameObject);
                break;
        }
        
    }

    //Check the plank's state and the ball's state, then destroy/interact with plank
    private void plankCollision(GameObject plank)
    {
        Debug.Log("Ball Collided with plank");
        //TODO some property check here
        if(tempState == StateReference.temperature.hot)
        {
            Plank plankProperties = plank.gameObject.GetComponent<Plank>();
            switch(plankProperties.plankState)
            {
                case StateReference.temperature.cold:
                    tempState = StateReference.temperature.neutral;
                    ballDisplay.material.color = Color.gray;
                    plank.SetActive(false);
                    break;
                case StateReference.temperature.hot:
                    break;
                case StateReference.temperature.neutral:
                    break;
            }
        }
    }
    private void staticplankCollision(GameObject plank)
    {
        Debug.Log("Ball Collided with plank");
        //TODO some property check here
        if (tempState == StateReference.temperature.hot)
        {
            Static_Plank plankProperties = plank.gameObject.GetComponent<Static_Plank>();
            switch (plankProperties.plankState)
            {
                case StateReference.temperature.cold:
                    tempState = StateReference.temperature.neutral;
                    ballDisplay.material.color = Color.gray;
                    plank.SetActive(false);
                    break;
                case StateReference.temperature.hot:
                    break;
                case StateReference.temperature.neutral:
                    break;
            }
        }
    }
    private void elementCollision(GameObject element)
    {
        ChangeTemperature elementProperties = element.GetComponent<ChangeTemperature>();
        switch (elementProperties.setting)
        {
            case StateReference.temperature.hot:
                Debug.Log("Collided with heater");
                tempState = StateReference.temperature.hot;
                ballDisplay.material.color = Color.red;
                break;
            //Add more cases here
        }
    }

    private void checkpointCollision(GameObject checkpoint)
    {
        GoalBlock goal = checkpoint.GetComponent<GoalBlock>();
        char color;
        switch (goal.goalColor)
        {
            case StateReference.goalColor.purple:
                color = 'p';
                break;
            case StateReference.goalColor.yellow:
                color = 'y';
                break;
            case StateReference.goalColor.white:
                color = 'w';
                break;
            case StateReference.goalColor.green:
                color = 'g';
                break;
            default:
                color = 'z';
                break;
        }
        Debug.Log("Collided with checkpoint. It has color " + color);
        DungeonMaster.dm.checkpointHit(color);
        checkpoint.SetActive(false);
    }

    //Enable physics when the user presses the start button
    public void startSim()
    {
        Debug.Log("Ball: simulation Started");
        ballPhysics.constraints = RigidbodyConstraints2D.None;
        ballPhysics.isKinematic = false;
        //ballPhysics.transform.position = startPosition;
    }

    public void stopSim()
    {
        Debug.Log("Ball: simulaton stopped");
        ballPhysics.constraints = RigidbodyConstraints2D.FreezePosition;
        ballPhysics.isKinematic = true;
        transform.position = startPosition;
    }
}
