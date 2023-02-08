using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plank : MonoBehaviour
{ 
    //Outlets
    public StateReference.temperature plankState;
    private SpriteRenderer plankDisplay;
    public bool editable = true;

    // variable to track simulation mode
    bool simulationMode;


    // variables needed to drag and drop toolkit items
    bool canMove;
    bool dragging;
    Collider2D _collider;

    // to rotate toolkit items
    public float rotationSpeed = 20f;

    //variable to create new gameObject
    public GameObject originalObject;
    public GameObject currentInstance;



    void Start()
    {

        
        // variables needed to move PLANK 
        _collider = GetComponent<Collider2D>();
        canMove = false;
        dragging = false;

        //Based on its current temperatue, set the color
        plankDisplay = GetComponent<SpriteRenderer>();
        /*
        switch (plankState)
        {
            case StateReference.temperature.neutral:
                plankDisplay.material.color = Color.green;
                break;
            case StateReference.temperature.cold:
                plankDisplay.material.color = Color.cyan;
                break;
            case StateReference.temperature.hot:
                plankDisplay.material.color = Color.red;
                break;
        }*/
        plankDisplay.material.color = new Color(243,145,1);
    }

    // Update is called once per frame
    void Update()
    {

        /*----- Below Code Segment for DRAG & DROP functionality -----*/
        // find mouse position in case of dragging
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 
        simulationMode = DungeonMaster.dm.GetStatusOfSimulationMode();

        // mouse down event check

        // accessing the simulationMode variable from Dungeon Master

        if (Input.GetMouseButtonDown(0) && !simulationMode)
        {
            Vector3 mousePositionOnClickedObject = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePositionOnClickedObject2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePositionOnClickedObject2D, Vector2.zero);
            if (hit.collider != null)
            {
                currentInstance = hit.collider.gameObject;
                Debug.Log(currentInstance.tag);
            }
            if(currentInstance!= null && currentInstance.tag == "Plank" )
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




        /*------- Below Code Segment to Rotate a toolkit item -----*/
        if(currentInstance!= null && currentInstance.tag == "Plank" && !simulationMode)
        {
            if ( Input.GetKey(KeyCode.RightArrow) )
            {
                currentInstance.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                currentInstance.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace))
            {
                destroyToolObject(currentInstance);
            }
        }

        /*--------------------------------------------------------*/

    }

    /*------- Below Code Segment to create a new toolkit item -----*/
    public void createInstance()
    {
        if (!simulationMode)
        {
            Debug.Log("clicked Plank Button");
            GameObject newPlank = Instantiate(originalObject, new Vector3(0,0, 0), Quaternion.identity);
            newPlank.GetComponent<Plank>().plankState = StateReference.temperature.neutral;
        }
        else
        {
            Debug.Log("In Editing Mode hence NO PLANK created!");
        }
    }
    /*------------------------------------------------------------*/


    private void destroyToolObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

}