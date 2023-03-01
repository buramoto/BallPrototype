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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D ballPhys = collision.gameObject.GetComponent<Rigidbody2D>();
        ballPhys.AddForce(new Vector2(-springForce * Mathf.Sin(transform.rotation.z), springForce * Mathf.Cos(transform.rotation.z)));
    }
}
