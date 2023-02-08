using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTemperature : MonoBehaviour
{
    /*
     * This script handles the properties of the heating/cooling element.
     * Its type can be specified in the Unity UI or dynamically with a script
    */

    public StateReference.temperature setting;
    public GameObject referenceToOriginalHeaterObject;
    private GameObject currentInstance;

    // variable to track simulation mode
    bool simulationMode;

    bool canMove;
    bool dragging;
    private Collider2D _collider;

    private SpriteRenderer elementDisplay;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        elementDisplay = GetComponent<SpriteRenderer>();
        switch (setting)
        {
            case StateReference.temperature.neutral:
                elementDisplay.material.color = Color.green;
                break;
            case StateReference.temperature.hot:
                elementDisplay.material.color = Color.red;
                break;
            case StateReference.temperature.cold:
                elementDisplay.material.color = Color.cyan;
                break;
        }

        _collider = GetComponent<Collider2D>();
        canMove = false;
        dragging = false;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        simulationMode = DungeonMaster.dm.GetStatusOfSimulationMode();
        // mouse down event check
        Debug.Log("Heater Status of Simulation Mode: "+simulationMode);
        if (Input.GetMouseButtonDown(0) && !simulationMode)
        {
            Vector3 mousePositionOnClickedObject = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePositionOnClickedObject2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePositionOnClickedObject2D, Vector2.zero);
            if (hit.collider != null)
            {
                currentInstance = hit.collider.gameObject;
            }

            if(currentInstance!= null && currentInstance.tag == "TempChange")
            {
                if (_collider == Physics2D.OverlapPoint(mousePos))
                {
                    canMove = true;
                    //Debug.Log("clicked on item");
                }
                else
                {
                    canMove = false;
                }
                if (canMove)
                {
                    dragging = true;
                }
            }
        }
        if (dragging && !simulationMode)
        {
            // updating the position of the toolkit item to mouse's current position
            currentInstance.transform.position = mousePos;
        }
        if (Input.GetMouseButtonUp(0) && !simulationMode)
        {
            canMove = false;
            dragging = false;
        }
        /*---------------------------------------------------------*/


        if(currentInstance != null && currentInstance.tag == "TempChange" && !simulationMode)
        {
            /*------- Below Code Segment to DELETE a SPRING  -----------*/
            if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace))
            {
                destroyToolObject(currentInstance);
            }
            /*---------------------------------------------------------*/
        }


        
    }

    /*------- Below Code Segment to create a new toolkit item -----*/
    public void createInstance()
    {
        simulationMode = DungeonMaster.dm.GetStatusOfSimulationMode();
        if (!simulationMode)
        {
            Debug.Log("New Heater Object Created");
            Instantiate(referenceToOriginalHeaterObject, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            Debug.Log("In Editing Mode hence no heater created!");
        }
    }
    /*------------------------------------------------------------*/




    /*------- Below FUNCTION IS TO DELETE SPRING -----------------*/
    private void destroyToolObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    /*------------------------------------------------------------*/
}
