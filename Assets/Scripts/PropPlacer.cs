using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropPlacer : MonoBehaviour
{
    //Static reference
    public static PropPlacer propPlacer;

    //References to objects
    public GameObject plankPrefab;
    public GameObject springPrefab;
    public GameObject tempElementPrefab;

    //For display purposes
    public TMPro.TextMeshProUGUI text;

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
        text.text = "Resources:\n"+DungeonMaster.dm.resources;
    }

    public void createPlank()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode && (DungeonMaster.dm.resources - DungeonMaster.dm.plankCost >= 0))
        {
            // Debug.Log("Creating Plank");
            
            // Increment the plank counter
            GlobalVariables.plankCounter++;
            Debug.Log("Plank Counter: " + GlobalVariables.plankCounter);

            GameObject newPlank = Instantiate(plankPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Plank plankScript = newPlank.GetComponent<Plank>();
            plankScript.ChangeTemp(StateReference.temperature.neutral);
            plankScript.editable = true;
            DungeonMaster.dm.resources = DungeonMaster.dm.resources - DungeonMaster.dm.plankCost;
        }
        else if(DungeonMaster.dm.resources < DungeonMaster.dm.plankCost)
        {
            Debug.LogWarning("Not enough reseources for a Plank");
        }
        else
        {
            Debug.LogWarning("In Editing Mode hence NO plank created!");
        }
    }

    public void createSpring()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode && (DungeonMaster.dm.resources - DungeonMaster.dm.springCost >= 0))
        {
            Debug.Log("Creating Spring");

            // Increment the spring counter
            GlobalVariables.springCounter++;
            Debug.Log("Spring Counter: " + GlobalVariables.springCounter);

            GameObject newSpring = Instantiate(springPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Spring springScript = newSpring.GetComponent<Spring>();
            springScript.editable = true;
            DungeonMaster.dm.resources = DungeonMaster.dm.resources - DungeonMaster.dm.springCost;
        }
        else if (DungeonMaster.dm.resources < DungeonMaster.dm.plankCost)
        {
            Debug.LogWarning("Not enough reseources for a Spring");
        }
        else
        {
            Debug.LogError("In Editing Mode hence NO spring created!");
        }
    }

    public void createTempElement()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode && (DungeonMaster.dm.resources - DungeonMaster.dm.elementCost >= 0))
        {
            Debug.Log("Creating Temperature Element");

            // Increment the heater counter
            GlobalVariables.heaterCounter++;
            Debug.Log("Heater Counter: " + GlobalVariables.heaterCounter);
            
            GameObject newTempElement = Instantiate(tempElementPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            ChangeTemperature newTempElementScript = newTempElement.GetComponent<ChangeTemperature>();
            newTempElementScript.ChangeTemp(StateReference.temperature.hot);
            newTempElementScript.editable = true;
            DungeonMaster.dm.resources = DungeonMaster.dm.resources - DungeonMaster.dm.elementCost;
        }
        else if (DungeonMaster.dm.resources < DungeonMaster.dm.plankCost)
        {
            Debug.LogWarning("Not enough reseources for an Element");
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
