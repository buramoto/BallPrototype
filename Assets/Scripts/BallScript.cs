using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private Camera cam; // Reference to the main camera in the scene

    // Variables to store the height and width of the screen
    private float screenHeight;
    private float screenWidth;

    // Ball is not editable neither during Simulation nor during Editing Phase
    public bool editable = false;


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

        // Get the reference to the main camera
        cam = Camera.main;

        // Calculate the height and width of the screen
        screenHeight = 2f * cam.orthographicSize;
        screenWidth = screenHeight * cam.aspect;

        stopSim();
    }

    private void Update()
    {
        // Get the x and y positions of the ball
        float ballX = transform.position.x;
        float ballY = transform.position.y;

        // Check if the ball is outside the bounds of the screen
        if (ballX < -screenWidth / 2 || ballX > screenWidth / 2 || ballY < -screenHeight / 2 || ballY > screenHeight / 2)
        {
            // If the ball is outside the bounds, call the changeMode() function
            DungeonMaster.dm.simMode(false, StateReference.resetType.oob);
            UIBehavior.gameUI.oobCoords = transform.position;
        }
    }

    //When colliding with an object, invoke appropriate function
    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Plank":
                plankCollision(collision.gameObject);
                break;
            case "Static_Plank":
                Debug.LogError("Static Plank Reference!");
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
    //It is a matrix of interactions: hh, hn, hc, nh, nn, nc, ch, cn, cc
    private void plankCollision(GameObject plank)
    {
        Plank plankProperties = plank.gameObject.GetComponent<Plank>();
        switch (tempState)
        {
            case StateReference.temperature.hot:
                switch (plankProperties.plankState)
                {
                    case StateReference.temperature.cold://Hot -> cold
                        tempState = StateReference.temperature.neutral;
                        ballDisplay.material.color = Color.gray;
                        plank.SetActive(false);
                        break;
                    case StateReference.temperature.neutral://Hot -> neutral
                        break;
                    case StateReference.temperature.hot://Hot -> hot
                        break;
                }
                break;
            case StateReference.temperature.neutral:
                break;
            default:
                break;
        }
    }

    private void elementCollision(GameObject element)
    {
        if(!DungeonMaster.dm.simulationMode) {
            Debug.Log("Collision in simulation");
            return;         
        }
        else{
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
    }

    private void checkpointCollision(GameObject checkpoint)
    {
        if(!DungeonMaster.dm.simulationMode) {
            Debug.Log("Collision in simulation");
            return;         
        }
        else{
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
            checkpoint.SetActive(false);
            DungeonMaster.dm.checkpointHit(color);
        }
    }

    //Enable physics when the user presses the start button
    //We should change this to an event
    public void startSim()
    {
        //Debug.Log("Ball: simulation Started");
        ballPhysics.constraints = RigidbodyConstraints2D.None;
        ballPhysics.isKinematic = false;
        //ballPhysics.transform.position = startPosition;
    }

    public void stopSim()
    {
        //Debug.Log("Ball: simulaton stopped");
        ballPhysics.constraints = RigidbodyConstraints2D.FreezePosition;
        ballPhysics.isKinematic = true;
        transform.position = startPosition;
        tempState = StateReference.temperature.neutral;
        ballDisplay.material.color = Color.gray;
    }
}
