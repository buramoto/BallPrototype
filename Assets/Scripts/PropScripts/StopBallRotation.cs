using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBallRotation : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Get the current rotation angle of the ball
        float angle = transform.rotation.eulerAngles.z;

        // Apply a counter-rotation to cancel out any rotation caused by the physics engine
        rb.AddTorque(-angle * rb.angularDrag);
    }
}

