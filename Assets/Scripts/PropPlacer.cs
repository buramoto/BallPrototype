using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PropPlacer : MonoBehaviour
{
    //Static reference
    public static PropPlacer propPlacer;
    public static Vector2 mousePosition;

    //References to objects
    public GameObject plankPrefab;
    public GameObject springPrefab;
    public GameObject tempElementPrefab;
    public GameObject converttoSteelPrefab;
    public GameObject converttoWoodPrefab;
    public Camera mainCam;
    public GameObject operationsPanelPrefab;
    private GameObject operationsPanel = null;
    private RectTransform panel;

    //Private variables
    private bool dragging;
    private Vector2 offset = new Vector2(0f,0f);
    public GameObject selectedObject; //Make this private when debugging is done
    public Vector3 positionBeforeClicking;
    public Quaternion rotationBeforeClicking;
    public Collider2D collidingObj;
    private bool isNew;

    //Settings
    // public const float rotationSpeed = 500; //Now turned into a constant field

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
        DungeonMaster.dm.StartSim += transitionToSimMode;
    }

    // Update is called once per frame
    void Update()
    {
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
                    Destroy(operationsPanel);
                    operationsPanel = null;
                }
                selectedObject = null;
                return;
            }
            
            GameObject clickedObject = hit.collider.gameObject;
            //Debug.Log("__________/------RayCast Hit's: "+clickedObject.tag);
            //Debug.Log("__________/------RayCast Hit's: " + hit);

            if (clickedObject.gameObject.layer == 5)
            {
                //Check if we have clicked on any UI elements
                selectedObject = null;
                DungeonMaster.dm.RemoveHighlightFromObject();
                Destroy(operationsPanel);
                operationsPanel = null;
                return;
            }
            if (clickedObject.tag == "Cannon" || clickedObject.tag == "Barrel" || clickedObject.tag == "Airflow" || clickedObject.tag == "WaterBody" || clickedObject.tag == "SpringPref" || clickedObject.tag == "WaterPref"  ) {
                DungeonMaster.dm.RemoveHighlightFromObject();
                selectedObject = null;
                return;
            }
            if( (clickedObject.tag=="Plank" && clickedObject.GetComponent<Plank>().editable) || clickedObject.tag == "Spring" || clickedObject.tag == "TempChange" || clickedObject.tag=="MaterialChange"){
                DungeonMaster.dm.HighlightObject(clickedObject);
                offset = (Vector2)clickedObject.transform.position - mousePosition;
                positionBeforeClicking = clickedObject.transform.position;
                rotationBeforeClicking = clickedObject.transform.rotation;
                Debug.Log("MouseDown on : " + clickedObject.tag);
                //Creating the toolkit button
                //Debug.Log("Creating dynamic operations panel");

                // IMPORTANT: This is the code that creates the operations panel on the highlighted object, please uncomment this code when you are done with the operations panel
                if(operationsPanel == null)
                {
                    operationsPanel = Instantiate(operationsPanelPrefab);
                    panel = operationsPanel.transform.Find("Operations").gameObject.GetComponent<RectTransform>();
                    if(clickedObject.CompareTag("TempChange") || (clickedObject.tag == "MaterialChange"))
                    {
                        panel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
                        panel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);
                        //panel.GetComponent<HorizontalLayoutGroup>().padding.left = 35;
                        //panel.GetComponent<HorizontalLayoutGroup>().padding.right = 35;
                    }
                    else
                    {
                        panel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
                        panel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
                        //panel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
                        //panel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;
                    }
                }
                else
                {
                    panel = operationsPanel.transform.Find("Operations").gameObject.GetComponent<RectTransform>();
                    if (clickedObject.CompareTag("TempChange") || (clickedObject.tag == "MaterialChange"))
                    {
                        panel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
                        panel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(false);
                        panel.GetComponent<HorizontalLayoutGroup>().padding.left = 30;
                        panel.GetComponent<HorizontalLayoutGroup>().padding.right = 30;
                    }
                    else
                    {
                        panel.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
                        panel.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive(true);
                        panel.GetComponent<HorizontalLayoutGroup>().padding.left = 5;
                        panel.GetComponent<HorizontalLayoutGroup>().padding.right = 5;
                    }
                }
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
            if (clickedObject.tag == "Plank" || clickedObject.tag == "Spring")
            {
                clickedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                clickedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            selectedObject = clickedObject;
        }
        //We are still dragging, so update position based on mouse position
        if (dragging)
        {
            //Debug.Log(offset);
            selectedObject.transform.position = mousePosition + offset;
        }
        //Player has released m1, stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            //isOffsetCalculated = false;
            if (selectedObject != null)
            {
                Debug.Log("Selected object is:-" + selectedObject);
                Debug.Log("Position of the selected object" + selectedObject.transform.position);
                if (selectedObject.CompareTag("Plank"))
                {
                    Plank p1 = selectedObject.GetComponent<Plank>();
                    Debug.Log("Checking the object p1" + p1);

                    if (p1.isOverlapping() == true)
                    {
                        Debug.Log("Object is colliding");
                        if (isNew)
                        {
                            deleteToolkitInstance();
                            selectedObject = null;
                        }
                        else
                        {
                            selectedObject.transform.position = positionBeforeClicking;
                            selectedObject.transform.rotation = rotationBeforeClicking;
                        }
                        isNew = false;
                        dragging = false;
                    }
                    else
                    {
                        Debug.Log("No Object is colliding successful positioning");
                        dragging = false;
                    }
                    isNew = false;
                    if (selectedObject != null) {
                        selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    }

                }
                else if(selectedObject.CompareTag("Spring"))
                {
                    Spring p1 = selectedObject.GetComponent<Spring>();
                    Debug.Log("Checking the object p1 " + p1);

                    if (p1.isOverlapping() == true)
                    {
                        Debug.Log("Object is colliding");
                        if(isNew)
                        {
                            deleteToolkitInstance();
                            isNew = false;
                            selectedObject = null;
                        }
                        else
                        {
                            selectedObject.transform.position = positionBeforeClicking;
                            selectedObject.transform.rotation = rotationBeforeClicking;
                        }
                        dragging = false;
                    }
                    else
                    {
                        Debug.Log("No Object is colliding successful positioning");
                        dragging = false;
                        isNew = false;
                    }

                    if (selectedObject != null)
                    {
                        selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
                else if(selectedObject.CompareTag("TempChange"))
                {
                    ChangeTemperature p1 = selectedObject.GetComponent<ChangeTemperature>();
                    Debug.Log("Checking the object p1" + p1);

                    if (p1.isOverlapping() == true)
                    {
                        Debug.Log("Object is colliding");
                        if (isNew)
                        {
                            deleteToolkitInstance();
                            isNew = false;
                            selectedObject = null;
                        }
                        else
                        {
                            selectedObject.transform.position = positionBeforeClicking;
                            selectedObject.transform.rotation = rotationBeforeClicking;
                        }
                        dragging = false;
                    }
                    else
                    {
                        Debug.Log("No Object is colliding successful positioning");
                        dragging = false;
                        isNew = false;
                    }
                    //selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    if (selectedObject != null)
                    {
                        selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
                else
                {
                    if (selectedObject != null)
                    {
                        selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        selectedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                    dragging = false;
                }/*
                else if (selectedObject.CompareTag("TempChange"))
                {
                    dragging = false;
                }
                else if (selectedObject.CompareTag("TempChange"))
                {
                    dragging = false;
                }  */  

               //dragging = false;
            }
        }

        //Positioning of operations panel
        if(selectedObject != null)
        {
            Bounds propCorners = selectedObject.GetComponent<Collider2D>().bounds;
            //Vector3 panelOffset = new Vector3(0, propCorners.center.y - propCorners.extents.y, 0);
            Vector3 panelOffset = new Vector3(0, propCorners.extents.y + 0.5f, 0);
            if(panel && panelOffset!=null)
            {
                panel.transform.position = mainCam.WorldToScreenPoint(selectedObject.transform.position - panelOffset);
            }
            //panel.anchorMax = mainCam.WorldToScreenPoint(selectedObject.transform.position);
        }

        // if (GlobalVariables.heaterCap > 0)
        // {
        //     GameObject.Find("Element").GetComponent<Button>().interactable = true;
        // }

        // if (GlobalVariables.plankCap > 0)
        // {
        //     GameObject.Find("Plank").GetComponent<Button>().interactable = true;
        // }

        // if (GlobalVariables.springCap > 0)
        // {
        //     GameObject.Find("Spring").GetComponent<Button>().interactable = true;
        // }
    }

    public void transitionToSimMode()
    {
        selectedObject = null;
    }

    public void createPlank()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode && GlobalVariables.plankCap > 0)
        {
            // Increment the plank counter
            GlobalVariables.plankCounter++;
            GameObject newPlank = Instantiate(plankPrefab, mousePosition, Quaternion.identity);
            Plank plankScript = newPlank.GetComponent<Plank>();
            plankScript.ChangeTemp(StateReference.temperature.neutral);
            plankScript.editable = true;
            plankScript.hasCollided = false;
            isNew = true;
            GlobalVariables.plankCap -= 1;
            if (GlobalVariables.plankCap == 0)
            {
                GameObject.Find("Plank").GetComponent<Button>().interactable = false;
                return;
            }
        }
    }

    public void createSpring()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode && GlobalVariables.springCap > 0)
        {
            Debug.Log("A Spring has been created");
            // Increment the spring counter
            GlobalVariables.springCounter++;
            GameObject newSpring = Instantiate(springPrefab, mousePosition, Quaternion.identity);
            Spring springScript = newSpring.GetComponent<Spring>();
            springScript.editable = true;
            springScript.hasCollided = false;
            springScript.spriteRenderer = newSpring.GetComponent<SpriteRenderer>();
            isNew = true;
            GlobalVariables.springCap -= 1;
            if (GlobalVariables.springCap == 0)
            {
                GameObject.Find("Spring").GetComponent<Button>().interactable = false;
                return;
            }
        }
    }

    public void createTempElement()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode && GlobalVariables.heaterCap > 0)
        {
            // Increment the heater counter
            GlobalVariables.heaterCounter++;
            GameObject newTempElement = Instantiate(tempElementPrefab, mousePosition, Quaternion.identity);
            Debug.Log("Creating plank at: " + mousePosition);
            ChangeTemperature newTempElementScript = newTempElement.GetComponent<ChangeTemperature>();
            newTempElementScript.ChangeTemp(StateReference.temperature.hot);
            newTempElementScript.editable = true;
            newTempElementScript.hasCollided = false;
            isNew = true;
            Debug.Log("GlobalVariables.heaterCap: " + GlobalVariables.heaterCap);
            
            GlobalVariables.heaterCap -= 1;
            if (GlobalVariables.heaterCap == 0)
            {
                GameObject.Find("Element").GetComponent<Button>().interactable = false;
                return;
            }
        }
    }

    public void converttoSteel()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode )
        {
            // Increment the heater counter
            GlobalVariables.convertercount++;
            GameObject newTempElement = Instantiate(converttoSteelPrefab, mousePosition, Quaternion.identity);
            Debug.Log("Creating convert to steel at: " + mousePosition);
            MaterialChange newmaterialChangeScript = newTempElement.GetComponent<MaterialChange>();
            newmaterialChangeScript.ChangeTemp(StateReference.temperature.hot);
            newmaterialChangeScript.editable = true;
            newmaterialChangeScript.hasCollided = false;
            isNew = true;
            //Debug.Log("GlobalVariables.heaterCap: " + GlobalVariables.heaterCap);
            /*
            GlobalVariables.heaterCap -= 1;
            if (GlobalVariables.heaterCap == 0)
            {
                GameObject.Find("Element").GetComponent<Button>().interactable = false;
                return;
            }*/
        }
    }

    public void converttoWood()
    {
        DungeonMaster.dm.RemoveHighlightFromObject();
        if (!DungeonMaster.dm.simulationMode)
        {
            // Increment the heater counter
            GameObject newTempElement = Instantiate(converttoWoodPrefab, mousePosition, Quaternion.identity);
            Debug.Log("Creating convert to Wood at: " + mousePosition);

            MaterialChange newmaterialChangeScript = newTempElement.GetComponent<MaterialChange>();
            newmaterialChangeScript.ChangeTemp(StateReference.temperature.hot);
            newmaterialChangeScript.editable = true;
            newmaterialChangeScript.hasCollided = false;
            isNew = true;
            /*
            Debug.Log("GlobalVariables.heaterCap: " + GlobalVariables.heaterCap);

            GlobalVariables.heaterCap -= 1;
            if (GlobalVariables.heaterCap == 0)
            {
                GameObject.Find("Element").GetComponent<Button>().interactable = false;
                return;
            }
            */
        }
    }


    public void deleteToolkitInstance(){
        GameObject toolkitInstance = DungeonMaster.dm.highlightedObject;
        if(toolkitInstance.tag == "Plank")
        {
            GlobalVariables.plankCounter--;
            GlobalVariables.plankCap += 1;
            if (GlobalVariables.plankCap > 0)
            {
                GameObject.Find("Plank").GetComponent<Button>().interactable = true;
            }
            if(toolkitInstance.GetComponent<Plank>().hasCollided)
            {
                GlobalVariables.plankUsed--;
            }
        }
        if(toolkitInstance.tag == "Spring")
        {
            GlobalVariables.springCounter--;
            GlobalVariables.springCap += 1;
            if (GlobalVariables.springCap > 0)
            {
                GameObject.Find("Spring").GetComponent<Button>().interactable = true;
            }
            if(toolkitInstance.GetComponent<Spring>().hasCollided)
            {
                GlobalVariables.springUsed--;
            }
        }
        if(toolkitInstance.tag == "TempChange")
        {
            GlobalVariables.heaterCounter--;
            GlobalVariables.heaterCap += 1;
            if (GlobalVariables.heaterCap > 0)
            {
                GameObject.Find("Element").GetComponent<Button>().interactable = true;
            }
            if(toolkitInstance.GetComponent<ChangeTemperature>().hasCollided)
            {
                GlobalVariables.heaterUsed--;
            }
        }
        //if (toolkitInstance.tag == "MaterialChange")
        //{

        //}
        Destroy(toolkitInstance);
        DungeonMaster.dm.RemoveHighlightFromObject();
        Destroy(operationsPanel);
        operationsPanel = null;
    }

    public void rotateLeft(float rotationSpeed){
        GameObject toolkitInstance = DungeonMaster.dm.highlightedObject;
        if(toolkitInstance.tag != "TempChange"){
            toolkitInstance.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    public void rotateRight(float rotationSpeed){
        GameObject toolkitInstance = DungeonMaster.dm.highlightedObject;
        if(toolkitInstance.tag != "TempChange"){
            toolkitInstance.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
    }

    public Vector3 worldToScreenPoint(Vector3 worldPosition)
    {
        return mainCam.WorldToScreenPoint(worldPosition);
    }
}
