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
    private CapsuleCollider2D sword; // variable for sword

    // Variables to store the height and width of the screen
    private float screenHeight;
    private float screenWidth;

    // Ball is not editable neither during Simulation nor during Editing Phase
    public bool editable = false;
    public Animator anim;

    // SwordHolder variable (parent of sword)
    public GameObject swordHolder; 

    // public bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        //if (gameObject.transform.GetChild(0).gameObject;) {

        //}
        swordHolder = gameObject.transform.GetChild(0).gameObject;
        // setting the ball's sword to inactive intially, when user clicks right mouse button only then collider comopenent will be set active
        if(gameObject.GetComponentInChildren<CapsuleCollider2D>() != null)
        {
            sword = gameObject.GetComponentInChildren<CapsuleCollider2D>();
            sword.enabled = false;
        }

        if(DungeonMaster.dm.currentSceneName == "Level7"){
            // GameObject dmInstance = FindAnyObjectByType<DungeonMaster>().gameObject;
            // dmInstance.AddComponent<Tutorial2>();
            // GameObject[] tut2GameObject = FindGameObjectsWithTag("Tutorial2Instructions");
            PropPlacer.instructionArray2 = GameObject.FindGameObjectsWithTag("Tutorial2Instructions");
        }

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
        anim = gameObject.GetComponentInChildren<Animator>();
        Debug.Log("Found the following animator");
        Debug.Log(anim);
        stopSim();
    }

    private void Update()
    {
        swordHolder.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
        // Get the x and y positions of the ball
        float ballX = transform.position.x;
        float ballY = transform.position.y;

        // Check if the ball is outside the bounds of the screen
        if (ballX < -screenWidth / 2 || ballX > screenWidth / 2 || ballY < -screenHeight / 2 || ballY > screenHeight / 2)
        {
            // If the ball is outside the bounds, call the changeMode() function
            // GlobalVariables.oobCounter++;

            // DungeonMaster.dm.resetValues();
            SendToGoogle.sendToGoogle.resetGlobalVariables("OOB");
            
            DungeonMaster.dm.simMode(false, StateReference.resetType.oob);
            if(DungeonMaster.dm.currentSceneName == "Level5"){
                GameObject.FindObjectOfType<Level5>().SendMessage("OutOfBounds", "oob");
            }
            if(DungeonMaster.dm.currentSceneName == "Level4"){
                GameObject.FindObjectOfType<Level4>().SendMessage("OutOfBounds", "oob");
            }
            UIBehavior.gameUI.oobCoords = transform.position;
            //DungeonMaster.dm.instructions.text = "Use The Tools To The Right To Direct The Ball &\nThen Click Start To Begin Ball's Motion";
        }

        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("Slice key pressed");
            if (DungeonMaster.dm.simulationMode)
            {
                sword = gameObject.GetComponentInChildren<CapsuleCollider2D>();
                sword.enabled = true;
                anim.SetTrigger("Slice");
                Debug.Log("about to call setSwordInActive");
                Invoke("setSwordInActive", 1500/1000f);
            }
        }
    }

    //When colliding with an object, invoke appropriate function
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Debug.LogWarning("Ball collided with " + collision.gameObject.name);
        switch (collision.gameObject.tag)
        {
            case "Plank":
                plankCollision(collision.gameObject);
                break;
            case "Spring":
                if(collision.gameObject.GetComponent<Spring>().hasCollided == false)
                {
                    GlobalVariables.springUsed++;
                    GlobalVariables.usedSpringObjects.Add(collision.gameObject);
                    collision.gameObject.GetComponent<Spring>().hasCollided = true;
                    Debug.Log("Spring used "+GlobalVariables.springUsed);
                }
                // Debug.Log("Spring used"+GlobalVariables.springUsed);
                break;
            case "Static_Plank":
                Debug.LogError("Static Plank Reference!");
                break;
        }
    }

    //Change temperature based on heating/cooling element
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning("Ball entered trigger " + other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "Checkpoint":
                checkpointCollision(other.gameObject);
                break;
            case "TempChange":
                elementCollision(other.gameObject);
                if(other.gameObject.tag == "TempChange" && other.GetComponent<ChangeTemperature>().hasCollided == false && DungeonMaster.dm.simulationMode)
                {   
                    GlobalVariables.heaterUsed++;
                    GlobalVariables.usedHeaterObjects.Add(other.gameObject);
                    other.GetComponent<ChangeTemperature>().hasCollided = true;
                    Debug.Log("Heater used "+GlobalVariables.heaterUsed);
                }
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
        if(plankProperties.editable == true && plankProperties.hasCollided == false)
        {
            GlobalVariables.plankUsed++;
            Debug.Log("Plank used"+GlobalVariables.plankUsed);
            GlobalVariables.usedPlankObjects.Add(plank.gameObject);
            plankProperties.hasCollided = true;
        }
    }

    private void elementCollision(GameObject element)
    {
        if(!DungeonMaster.dm.simulationMode) {
            // Debug.Log("Collision in simulation");
            return;         
        }
        else{

            ChangeTemperature elementProperties = element.GetComponent<ChangeTemperature>();
            switch (elementProperties.setting)
            {
                case StateReference.temperature.hot:
                    Debug.Log("Collided with heater");


                    // Vector3 position = element.transform.position;
                    // GlobalVariables.heaterCoordinates += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
                    // Debug.Log("Heater Coordinates" + GlobalVariables.heaterCoordinates);

                    // adding heater element to list
                    // GlobalVariables.usedHeaterObjects.Add(element);
                    
                    // destroying heater element
                    element.SetActive(false);

                    tempState = StateReference.temperature.hot;
                    ballDisplay.material.color = Color.red;
                    break;
                //Add more cases here
            }
        }
    }

    private void checkpointCollision(GameObject checkpoint)
    {
        char color;
        GoalBlock goal = checkpoint.GetComponent<GoalBlock>();
        if(!DungeonMaster.dm.simulationMode) {
            Debug.Log("Collision in simulation");
            return;         
        }
        else{
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
            if(goal.goalColor != StateReference.goalColor.green) {
                checkpoint.SetActive(false);
            }
            DungeonMaster.dm.checkpointHit(checkpoint, color);
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

    public void setSwordInActive()
    {
        Debug.Log("Sword About to SET ENABLED = FALSE");
        sword = gameObject.GetComponentInChildren<CapsuleCollider2D>();
        sword.enabled = false;
    }
}
