using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Plank : MonoBehaviour
{ 
    //Outlets
    public StateReference.temperature plankState;
    public bool editable = true;
    public bool hasCollided = false;
    //Private variables
    private SpriteRenderer plankDisplay;

    //Sprites for the different temperatures
    public Sprite normPlankSprite;
    public Sprite coldPlankSprite;
    public Sprite hotPlankSprite;
    public int overLaps;

    void Start()
    {
        overLaps = 0;
        //Based on its current temperatue, set the color
        plankDisplay = GetComponent<SpriteRenderer>();
        if (!editable)
        {
            switch (plankState)
            {
                case StateReference.temperature.neutral:
                    plankDisplay.sprite = normPlankSprite;
                    break;
                case StateReference.temperature.cold:
                    plankDisplay.material.color = Color.cyan;
                    break;
                case StateReference.temperature.hot:
                    plankDisplay.material.color = Color.red;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        /*----- Below Code Segment for DRAG & DROP functionality -----
        // find mouse position in case of dragging
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 

        if (Input.GetMouseButtonDown(0) && !DungeonMaster.dm.simulationMode)
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
        if (dragging && !DungeonMaster.dm.simulationMode)
        {
            // updating the position of the toolkit item to mouse's current position
            currentInstance.transform.position = mousePos;
        }
        if (Input.GetMouseButtonUp(0) && !DungeonMaster.dm.simulationMode)
        {
            canMove = false;
            dragging = false;
        }
        ---------------------------------------------------------




        /*------- Below Code Segment to Rotate a toolkit item -----
        if(currentInstance!= null && currentInstance.tag == "Plank" && !DungeonMaster.dm.simulationMode)
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

        --------------------------------------------------------*/
        if(overLaps > 0 && editable == true && !DungeonMaster.dm.simulationMode)
        {
            plankDisplay.color = Color.magenta;
        }
        else
        {
            plankDisplay.color = Color.white;
        }

    }

    /*------- Below Code Segment to create a new toolkit item -----
    public void CreateInstance()
    {
        if (!DungeonMaster.dm.simulationMode)
        {
            Debug.Log("clicked Plank Button");
            GameObject newPlank = Instantiate(originalObject, new Vector3(0,0, 0), Quaternion.identity);
            newPlank.GetComponent<Plank>().plankState = StateReference.temperature.neutral;
            //We may need to switch out the plank sprite with different sprites depending on the temperature set
            SpriteRenderer newPlankRenderer = newPlank.GetComponent<SpriteRenderer>();
            newPlankRenderer.sprite = normPlankSprite;
        }
        else
        {
            Debug.Log("In Editing Mode hence NO PLANK created!");
        }
    }
    /*------------------------------------------------------------*/

    public void ChangeTemp(StateReference.temperature temp)
    {
        plankDisplay = GetComponent<SpriteRenderer>();
        plankState = temp;
        switch (temp)
        {
            case StateReference.temperature.neutral:
                plankDisplay.sprite = normPlankSprite;
                break;
            default:
                plankDisplay.sprite = normPlankSprite;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plank") || collision.gameObject.CompareTag("Spring") || collision.gameObject.CompareTag("TempChange"))
        {
            Debug.Log("This is the trigger enter stage Spring ");
            overLaps++;
            Debug.Log("Overlap value becomes" + overLaps);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plank") || collision.gameObject.CompareTag("Spring") || collision.gameObject.CompareTag("TempChange"))
        {
            Debug.Log("This is the trigger exit stage in Heater");
            overLaps--;
            Debug.Log("Overlap value becomes" + overLaps);
        }
    }
    public bool isOverlapping()
    {
        if (overLaps > 0)
            return true;
        return false;
    }
}