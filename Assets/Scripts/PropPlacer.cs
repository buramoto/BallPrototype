using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            //We are in simulation mode. Player should not be editing anything
            return;
        }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            selectedObject = null;
            dragging = false;
            if(hit.collider == null)
            {
                //Clicked on nothing; return
                return;
            }
            GameObject clickedObject = hit.collider.gameObject;
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
        if(selectedObject != null)
        {
            // Debug.Log(selectedObject.tag);
            if (Input.GetKey(KeyCode.RightArrow))//Rotate right
            {
                selectedObject.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow))//Rotate left
            {
                selectedObject.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace))//Delete
            {
                if(selectedObject.tag == "Plank")
                {
                    GlobalVariables.plankCounter--;
                    Debug.Log("Plank Counter after deletion: " + GlobalVariables.plankCounter);
                }
                if(selectedObject.tag == "Spring")
                {
                    GlobalVariables.springCounter--;
                    Debug.Log("Spring Counter after deletion: " + GlobalVariables.springCounter);
                }
                if(selectedObject.tag == "TempChange")
                {
                    GlobalVariables.heaterCounter--;
                    Debug.Log("Heater Counter after deletion: " + GlobalVariables.heaterCounter);
                }
                Destroy(selectedObject);
                selectedObject = null;
            }
        }
    }

    public void createPlank()
    {
        if (!DungeonMaster.dm.simulationMode)
        {
            Debug.Log("Creating Plank");

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
}
