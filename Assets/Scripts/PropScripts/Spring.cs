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
    public Sprite uncompressedSpringSprite;
    public Sprite outline;
    public SpriteRenderer spriteRenderer;
    public Animator anim;



    private void Start()
    {
        overLaps = 0;
        anim = gameObject.GetComponent<Animator>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("------ Collision Detected ---------");
        if (collision.gameObject.CompareTag("Player"))
        {
            int layerIndex = 0; // assuming you have only one layer in your Animator Controller

            // Check if the "compressSpring" trigger is already set
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(layerIndex);
            if (stateInfo.IsName("springDown") && stateInfo.normalizedTime < 0.3f)
            {
                Debug.Log("Spring is compressed right now so ball can't jump at this instant!");
                // The "beginSpringCompression" trigger is already set, so don't set it again
                return;
            }
            else
            {
                if (DungeonMaster.dm.simulationMode) {


                    anim.SetTrigger("beginSpringCompression");

                    Rigidbody2D ballPhys = collision.gameObject.GetComponent<Rigidbody2D>();

                    //ContactPoint2D[] contacts = new ContactPoint2D[1];
                    //collision.GetContacts(contacts);
                    //ContactPoint2D contact = contacts[0];
                    //Vector2 surfaceNormal = contact.normal;
                    //Vector2 bounceDirection = Vector2.Perpendicular(surfaceNormal).normalized;
                    //ballPhys.AddForce(bounceDirection * springForce, ForceMode2D.Impulse);

                    //ballPhys.AddForce(new Vector2(-springForce * Mathf.Sin(transform.rotation.z), springForce * Mathf.Cos(transform.rotation.z)));
                    ballPhys.AddForce(new Vector2(-springForce * Mathf.Sin(transform.rotation.z), springForce * Mathf.Cos(transform.rotation.z)), ForceMode2D.Impulse);
                    Debug.Log("Printing the value of ANIM: " + anim);
                }
            }


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
