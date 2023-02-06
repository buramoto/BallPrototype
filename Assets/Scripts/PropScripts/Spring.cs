using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    /*
     * This script handles the beahvior of the spring. The force can be specified externally
     * This script will need to be changed to check for collusion in a specific area instead of the entire object
    */
    //Outlets

    public float springForce;

    // Below variables are TO CREATE NEW INSTANCE OF SPRING
    public GameObject springObject;
    public GameObject currentInstance;

    // Below variables are TO MOVE SPRING USING DRAG & DROP 
    bool canMove;
    bool dragging;
    Collider2D _collider;

    // variable TO ROTATE SPRING
    private float rotationSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        // variables needed to move spring items
        _collider = GetComponent<Collider2D>();
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




        /*------- Below Code Segment to Rotate a SPRING -----*/
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentInstance.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentInstance.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
        /*---------------------------------------------------------*/



        /*------- Below Code Segment to DELETE a SPRING  -----------*/
        if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace))
        {
            destroyToolObject(currentInstance);
        }
        /*---------------------------------------------------------*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D ballPhys = collision.gameObject.GetComponent<Rigidbody2D>();
        ballPhys.AddForce(new Vector2(-springForce * Mathf.Sin(transform.rotation.z), springForce * Mathf.Cos(transform.rotation.z)));
    }


    /*------- Below Code Segment to create a new toolkit item -----*/
    public void createInstance()
    {
        Debug.Log("clicked Plank Button");
        Instantiate(springObject, new Vector3(0, 0, 0), Quaternion.identity);
    }
    /*------------------------------------------------------------*/




    /*------- Below FUNCTION IS TO DELETE SPRING -----------------*/
    private void destroyToolObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    /*------------------------------------------------------------*/

}
