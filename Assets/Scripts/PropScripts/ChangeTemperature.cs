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

    bool canMove;
    bool dragging;
    Collider2D collider;

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

        collider = GetComponent<Collider2D>();
        canMove = false;
        dragging = false;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // mouse down event check

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePositionOnClickedObject = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePositionOnClickedObject2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePositionOnClickedObject2D, Vector2.zero);
            if (hit.collider != null)
            {
                currentInstance = hit.collider.gameObject;
            }


            if (collider == Physics2D.OverlapPoint(mousePos))
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
        if (dragging)
        {
            // updating the position of the toolkit item to mouse's current position
            currentInstance.transform.position = mousePos;
        }
        if (Input.GetMouseButtonUp(0))
        {
            canMove = false;
            dragging = false;
        }
        /*---------------------------------------------------------*/



        /*------- Below Code Segment to DELETE a SPRING  -----------*/
        if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace))
        {
            destroyToolObject(currentInstance);
        }
        /*---------------------------------------------------------*/


        
    }

    /*------- Below Code Segment to create a new toolkit item -----*/
    public void createInstance()
    {
        Debug.Log("New Heater Object Created");
        Instantiate(referenceToOriginalHeaterObject, new Vector3(0, 0, 0), Quaternion.identity);
    }
    /*------------------------------------------------------------*/




    /*------- Below FUNCTION IS TO DELETE SPRING -----------------*/
    private void destroyToolObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    /*------------------------------------------------------------*/
}
