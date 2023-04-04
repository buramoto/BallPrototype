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
    public bool editable;
    public float springForce;
    public bool hasCollided = false;
    public int overLaps;

    private void Start()
    {
        overLaps = 0;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D ballPhys = collision.gameObject.GetComponent<Rigidbody2D>();
            ballPhys.AddForce(new Vector2(-springForce * Mathf.Sin(transform.rotation.z), springForce * Mathf.Cos(transform.rotation.z)));
        }
        else if (collision.gameObject.CompareTag("Plank") || collision.gameObject.CompareTag("Spring") || collision.gameObject.CompareTag("TempChange"))
        {
            Debug.Log("This is the trigger enter stage Spring ");
            overLaps++;
            Debug.Log("Overlap value becomes" + overLaps);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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
