using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProperties : MonoBehaviour
{
    /*
     * This script handles the behavior of the player's ball. It should handle collusions and interactions
     * such as destroying ice planks while hot
    */
    //State variables
    public enum temperature
    {
        neutral,
        hot,
        cold
    }

    //Outputs
    public GameObject ball;
    public temperature tempState;

    //Private variables
    private SpriteRenderer ballDisplay;
    

    // Start is called before the first frame update
    void Start()
    {
        //Set the ball to its starting position (This should be changed to be configurable based on level
        ball.transform.position = new Vector3(0, 0, 0);
        //Set inital temperature
        tempState = temperature.neutral;
        ball.SetActive(true);
        ballDisplay = GetComponent<SpriteRenderer>();
        ballDisplay.material.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When colliding with an object, invoke appropriate function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Plank":
                plankCollision(collision.gameObject);
                break;
        }
    }

    //Change temperature based on heating/cooling element
    private void OnTriggerEnter2D(Collider2D other)
    {
        ChangeTemperature elementProperties = other.gameObject.GetComponent<ChangeTemperature>();
        if(elementProperties.setting == ChangeTemperature.tempType.heater)
        {
            Debug.Log("Collided with heater");
            tempState = temperature.hot;
            ballDisplay.material.color = Color.red;
        }
    }

    //Check the plank's state and the ball's state, then destroy/interact with plank
    private void plankCollision(GameObject plank)
    {
        Debug.Log("Collided with plank");
        //TODO some property check here
        if(tempState == temperature.hot)
        {
            Plank plankProperties = plank.gameObject.GetComponent<Plank>();
            switch(plankProperties.plankState)
            {
                case Plank.plankTemp.ice:
                    tempState = temperature.neutral;
                    ballDisplay.material.color = Color.gray;
                    plank.SetActive(false);
                    break;
                case Plank.plankTemp.fire:
                    break;
                case Plank.plankTemp.normal:
                    break;
            }
        }
        return;
    }
}
