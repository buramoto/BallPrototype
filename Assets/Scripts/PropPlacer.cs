using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PropPlacer : MonoBehaviour
{
    //Static reference
    public static PropPlacer propPlacer;
    public static Vector2 mousePosition;

    //References to objects
    public GameObject plankPrefab;
    public GameObject springPrefab;
    public GameObject tempElementPrefab;
    public Camera mainCam;

    //Private variables
    private bool dragging;
    public GameObject selectedObject; //Make this private when debugging is done

    //Settings
    public const float rotationSpeed=500; //Now turned into a constant field

    private void initalizeLevel(Scene scene, LoadSceneMode mode)
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += initalizeLevel;
        selectedObject = null;
        dragging = false;
        propPlacer = this;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        //bool isOffsetCalculated = false;
        Vector2 offset = new Vector2(0f, 0f);

        if (DungeonMaster.dm.simulationMode)
        {
            //We are in simulation mode. Player should not be editing anything
            return;
        }


        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
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
                //Check if we have clicked on any UI elements
                selectedObject = null;
                DungeonMaster.dm.RemoveHighlightFromObject();
                return;
            }
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
            selectedObject.transform.position = mousePosition ;
        }
        //Player has released m1, stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            //isOffsetCalculated = false;
            dragging = false;
        }
    }

    public void createPlank()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode)
        {
            // Increment the plank counter
            GlobalVariables.plankCounter++;
            GameObject newPlank = Instantiate(plankPrefab, mousePosition, Quaternion.identity);
            Plank plankScript = newPlank.GetComponent<Plank>();
            plankScript.ChangeTemp(StateReference.temperature.neutral);
            plankScript.editable = true;
            plankScript.hasCollided = false;
        }
    }

    public void createSpring()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode)
        {
            // Increment the spring counter
            GlobalVariables.springCounter++;
            GameObject newSpring = Instantiate(springPrefab, mousePosition, Quaternion.identity);
            Spring springScript = newSpring.GetComponent<Spring>();
            springScript.editable = true;
            springScript.hasCollided = false;
        }
    }

    public void createTempElement()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode)
        {
            // Increment the heater counter
            GlobalVariables.heaterCounter++;
            GameObject newTempElement = Instantiate(tempElementPrefab, mousePosition, Quaternion.identity);
            Debug.Log("Creating plank at: " + mousePosition);
            ChangeTemperature newTempElementScript = newTempElement.GetComponent<ChangeTemperature>();
            newTempElementScript.ChangeTemp(StateReference.temperature.hot);
            newTempElementScript.editable = true;
            newTempElementScript.hasCollided = false;
        }
    }


    public void deleteToolkitInstance(){
        GameObject toolkitInstance = DungeonMaster.dm.highlightedObject;
        if(toolkitInstance.tag == "Plank")
        {
            GlobalVariables.plankCounter--;
            if(toolkitInstance.GetComponent<Plank>().hasCollided)
            {
                GlobalVariables.plankUsed--;
            }
        }
        if(toolkitInstance.tag == "Spring")
        {
            GlobalVariables.springCounter--;
            if(toolkitInstance.GetComponent<Spring>().hasCollided)
            {
                GlobalVariables.springUsed--;
            }
        }
        if(toolkitInstance.tag == "TempChange")
        {
            GlobalVariables.heaterCounter--;
            if(toolkitInstance.GetComponent<ChangeTemperature>().hasCollided)
            {
                GlobalVariables.heaterUsed--;
            }
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
