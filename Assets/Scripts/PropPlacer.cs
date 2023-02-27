using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PropPlacer : MonoBehaviour
{
    //Static reference
    public static PropPlacer propPlacer;

    //References to objects
    public GameObject plankPrefab;
    public GameObject springPrefab;
    public GameObject tempElementPrefab;

    //Private variables
    private bool dragging;
    public GameObject selectedObject; //Make this private when debugging is done

    //Settings
    private float rotationSpeed; //Derived from Dungeonmaster at start

    private void Start()
    {
        rotationSpeed = DungeonMaster.dm.rotationSpeed;
        selectedObject = null;
        dragging = false;
        propPlacer = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (DungeonMaster.dm.simulationMode)
        {
            // if user is in simulation mode and he presses "SPACE BAR" - For Next Instruction
            // Below If for TUTORIAL 1
            if (DungeonMaster.dm.tutorial1 == SceneManager.GetActiveScene().name) {
                if (Tutorial1.instructionIndex == 0)
                {
                    Tutorial1.UpdateInstructionIndex();
                }
                if (Tutorial1.instructionIndex == 1 && Input.GetKeyDown(KeyCode.Space))
                {
                    Tutorial1.tutorial1Reference.instructionArray[Tutorial1.instructionIndex - 1].SetActive(false);
                    Tutorial1.tutorial1Reference.instructionArray[Tutorial1.instructionIndex].SetActive(true);
                }
            }

            //We are in simulation mode. Player should not be editing anything
            return;
        }
        // when in editing mode we instruct user to place heater and a plank Below Code is for Tutorial 1
        if (DungeonMaster.dm.tutorial1 == SceneManager.GetActiveScene().name && !DungeonMaster.dm.simulationMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Tutorial1.instructionIndex == 1)
                {
                    Tutorial1.tutorial1Reference.instructionArray[0].SetActive(false);
                    Tutorial1.tutorial1Reference.instructionArray[1].SetActive(false);
                    Tutorial1.tutorial1Reference.instructionArray[2].SetActive(true);
                    Tutorial1.UpdateInstructionIndex();
                }
                else if (Tutorial1.instructionIndex == 2)
                {
                    Tutorial1.UpdateInstructionIndex();
                    Tutorial1.tutorial1Reference.instructionArray[Tutorial1.instructionIndex - 1].SetActive(false);
                    Tutorial1.tutorial1Reference.instructionArray[Tutorial1.instructionIndex].SetActive(true);
                }
                else if (Tutorial1.instructionIndex == 3)
                {
                    Tutorial1.UpdateInstructionIndex();
                    Tutorial1.tutorial1Reference.instructionArray[Tutorial1.instructionIndex - 1].SetActive(false);
                    Tutorial1.tutorial1Reference.instructionArray[Tutorial1.instructionIndex].SetActive(false);
                }
            }
        }

        if (DungeonMaster.dm.tutorial2 == SceneManager.GetActiveScene().name && !DungeonMaster.dm.simulationMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Tutorial2.instructionIndex == 0)
                {
                    Tutorial2.UpdateInstructionIndex();
                    Tutorial2.tutorial2Reference.instructionArray[Tutorial2.instructionIndex - 1].SetActive(false);
                    Tutorial2.tutorial2Reference.instructionArray[Tutorial2.instructionIndex].SetActive(true);
                }
                else if (Tutorial2.instructionIndex == 1)
                {
                    Tutorial2.UpdateInstructionIndex();
                    Tutorial2.tutorial2Reference.instructionArray[Tutorial2.instructionIndex - 1].SetActive(false);
                    Tutorial2.tutorial2Reference.instructionArray[Tutorial2.instructionIndex].SetActive(true);
                }
                else if (Tutorial2.instructionIndex == 2)
                {
                    Tutorial2.UpdateInstructionIndex();
                    Tutorial2.tutorial2Reference.instructionArray[Tutorial2.instructionIndex - 1].SetActive(false);
                    Tutorial2.tutorial2Reference.instructionArray[Tutorial2.instructionIndex].SetActive(false);
                }
            }
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            //selectedObject = null;
            //Debug.Log("PropPlacer just after RayCast Hit: "+hit.collider);

            // Debug.Log("Trying to identify using event system: "+EventSystem.current.currentSelectedGameObject );
            dragging = false;
            if(hit.collider == null)
            {
                if( EventSystem.current.currentSelectedGameObject !=null && (
                    EventSystem.current.currentSelectedGameObject.layer == 5)){
                    return;
                }
                if (DungeonMaster.dm.highlightedObject != null)
                {
                    DungeonMaster.dm.RemoveHighlightFromObject();
                }
                selectedObject = null;
                return;
            }
            
            GameObject clickedObject = hit.collider.gameObject;
            if(clickedObject.gameObject.layer == 5)
            {
                selectedObject = null;
                DungeonMaster.dm.RemoveHighlightFromObject();
                //Debug.LogError("Don't use this");
                return;
            }
            // Debug.Log("current Instance in PropPlacer: -----> "+clickedObject);
            if( (clickedObject.tag=="Plank" && clickedObject.GetComponent<Plank>().editable) || clickedObject.tag == "Spring" || clickedObject.tag == "TempChange" ){
                DungeonMaster.dm.HighlightObject(clickedObject);
                // below function set operation buttons to active mode when a correct object is clicked/highlighted
                
            }
            switch (clickedObject.tag) //Check to make sure what we clicked is editable. If it is not, return
            {
                case "Plank":
                    if (!clickedObject.GetComponent<Plank>().editable)
                    {
                        return;
                    }
                    break;
                case "Spring":
                    if (!clickedObject.GetComponent<Spring>().editable)
                    {
                        return;
                    }
                    break;
                case "TempChange":
                    if (!clickedObject.GetComponent<ChangeTemperature>().editable)
                    {
                        return;
                    }
                    break;
                case "Checkpoint":
                    return;
                    
                case "Player":
                    return ;
                    
            }
            dragging = true;
            selectedObject = clickedObject;
        }
        //We are still dragging, so update position based on mouse position
        if (dragging)
        {
            selectedObject.transform.position = mousePosition;
        }
        //Player has released m1, stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        //Other placement options for prop, but this should be done through UI later on 
        //Once we convert to UI controls, these methods should move to their own methods
        //So the UI can call the method
        // if(selectedObject != null)
        // {
        //     // Debug.Log(selectedObject.tag);
        //     if (Input.GetKey(KeyCode.RightArrow))//Rotate right
        //     {
        //         rotateRight(selectedObject);
        //     }
        //     if (Input.GetKey(KeyCode.LeftArrow))//Rotate left
        //     {
        //         rotateLeft(selectedObject);
        //     }
        //     if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace))//Delete
        //     {
        //         deleteToolkitInstance(selectedObject);
        //     }
        // }
    }

    public void createPlank()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode)
        {
            // Debug.Log("Creating Plank");

            // Increment the plank counter
            GlobalVariables.plankCounter++;
            Debug.Log("Plank Counter: " + GlobalVariables.plankCounter);

            GameObject newPlank = Instantiate(plankPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Plank plankScript = newPlank.GetComponent<Plank>();
            plankScript.ChangeTemp(StateReference.temperature.neutral);
            plankScript.editable = true;
        }
        else
        {
            Debug.LogWarning("In Editing Mode hence NO plank created!");
        }
    }

    public void createSpring()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode)
        {
            Debug.Log("Creating Spring");

            // Increment the spring counter
            GlobalVariables.springCounter++;
            Debug.Log("Spring Counter: " + GlobalVariables.springCounter);

            GameObject newSpring = Instantiate(springPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Spring springScript = newSpring.GetComponent<Spring>();
            springScript.editable = true;
        }
        else
        {
            Debug.LogError("In Editing Mode hence NO spring created!");
        }
    }

    public void createTempElement()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode)
        {
            Debug.Log("Creating Temperature Element");

            // Increment the heater counter
            GlobalVariables.heaterCounter++;
            Debug.Log("Heater Counter: " + GlobalVariables.heaterCounter);
            
            GameObject newTempElement = Instantiate(tempElementPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            ChangeTemperature newTempElementScript = newTempElement.GetComponent<ChangeTemperature>();
            newTempElementScript.ChangeTemp(StateReference.temperature.hot);
            newTempElementScript.editable = true;
        }
        else
        {
            Debug.LogError("In Editing Mode hence NO temperature element created!");
        }
    }


    public void deleteToolkitInstance(){
        GameObject toolkitInstance = DungeonMaster.dm.highlightedObject;
        if(toolkitInstance.tag == "Plank")
        {
            GlobalVariables.plankCounter--;
            Debug.Log("Plank Counter after deletion: " + GlobalVariables.plankCounter);
        }
        if(toolkitInstance.tag == "Spring")
        {
            GlobalVariables.springCounter--;
            Debug.Log("Spring Counter after deletion: " + GlobalVariables.springCounter);
        }
        if(toolkitInstance.tag == "TempChange")
        {
            GlobalVariables.heaterCounter--;
            Debug.Log("Heater Counter after deletion: " + GlobalVariables.heaterCounter);
        }
        Destroy(toolkitInstance);
        DungeonMaster.dm.RemoveHighlightFromObject();

    }

    public void rotateLeft(){
        GameObject toolkitInstance = DungeonMaster.dm.highlightedObject;
        if(toolkitInstance.tag != "TempChange"){
            toolkitInstance.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    public void rotateRight(){
        GameObject toolkitInstance = DungeonMaster.dm.highlightedObject;
        if(toolkitInstance.tag != "TempChange"){
            toolkitInstance.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
    }
}
