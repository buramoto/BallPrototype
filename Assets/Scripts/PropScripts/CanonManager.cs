using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CanonManager : MonoBehaviour
{
    // variables for GameObject References
    public GameObject cannonBallPrefab;
    public Transform firePoint;
    public LineRenderer lineRenderer;
    private Camera _cam;
    public GameObject barrelHolder;

    // variables for canon logic
    private const int N_TRAJECTORY_POINTS = 10;
    private bool _pressingMouse = false;
    private Vector3 _initialVelocity;
    public float velocityMultiplier=2f;
    private bool isCanonBallPresent = false;
    public float initialXPosition = 0;
    public float initialYPosition = -2;
    public float canonAutoRotateSpeed = 1;


    void Start()
    {
        _cam = DungeonMaster.dm.GetComponent<PropPlacer>().mainCam;

        lineRenderer.positionCount = N_TRAJECTORY_POINTS;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (DungeonMaster.dm.simulationMode)
        {
            // Add one more check that if the canon has the ball only then allow the below operations.
            if (isCanonBallPresent) {
                if (Input.GetMouseButtonDown(0))
                {
                    _pressingMouse = true;
                    lineRenderer.enabled = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    _pressingMouse = false;
                    lineRenderer.enabled = false;
                    _Fire();
                }

                if (_pressingMouse)
                {
                    // coordinate transform screen > world
                    Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0;

                    // look at
                    //transform.LookAt(mousePos);
                    //GameObject child = GameObject.FindWithTag("Firepoint");
                    float angle = Mathf.Atan2(mousePos.x - transform.position.x, mousePos.y - transform.position.y) * Mathf.Rad2Deg;
                    Debug.Log("ANGLE VALUE: ++++++++" + angle);
                    angle *= -1;

                    // set rotation of Cannon Barrel to face mouse position
                    barrelHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    // The ball is within the barrel hence change the all position too
                    cannonBallPrefab.transform.position = GameObject.FindWithTag("CannonBase").transform.position;

                    _initialVelocity = (mousePos - firePoint.position) * velocityMultiplier;

                    _UpdateLineRenderer();
                }
            }
       
        }
        
    }

    private void _Fire()
    {
        // instantiate a cannon ball
        //GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, Quaternion.identity);
        // apply some force
        Rigidbody2D rb = cannonBallPrefab.GetComponent<Rigidbody2D>();

        // Activating the forces
        rb.isKinematic = false;

        // unfreezing the rotation
        rb.constraints = RigidbodyConstraints2D.None;

        //changing the postion to the firepoint
        cannonBallPrefab.transform.position = firePoint.position;

        // Changing Ball's Temp to Red Hot when fired
        cannonBallPrefab.GetComponent<BallScript>().tempState = StateReference.temperature.hot;
        cannonBallPrefab.GetComponent<SpriteRenderer>().material.color = Color.red;

        // adding an impulse force to throw ball off the Canon
        rb.AddForce(_initialVelocity, ForceMode2D.Impulse);

        // setting Bool to False to denote that the ball is fired hence canon holds no ball within
        isCanonBallPresent = false;
        //cannonBallPrefab = null;

        // below code is to auto rotate the canon after firing the ball
        gameObject.transform.rotation = Quaternion.identity;
        //gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * canonAutoRotateSpeed);
    }

    private void _UpdateLineRenderer()
    {
        float g = Physics.gravity.magnitude;
        float velocity = _initialVelocity.magnitude;
        float angle = Mathf.Atan2(_initialVelocity.y, _initialVelocity.x);

        Vector3 start = firePoint.position;

        float timeStep = 0.1f;
        float fTime = 0f;
        for (int i = 0; i < N_TRAJECTORY_POINTS; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle);
            float dy = velocity * fTime * Mathf.Sin(angle) - (g * fTime * fTime / 2f);
            Vector3 pos = new Vector3(start.x + dx, start.y + dy, 0);
            lineRenderer.SetPosition(i, pos);
            fTime += timeStep;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // check if Ball has Collided with Canon
        if (other.CompareTag("Player"))
        {
            this.isCanonBallPresent = true;
            
            other.transform.position = GameObject.FindWithTag("CannonBase").transform.position;

            // disabling the gravity forces on the ball
            Rigidbody2D ballRigidBody = other.GetComponent<Rigidbody2D>();
            ballRigidBody.isKinematic = true;

            // Stopping the Ball's Rotation because the ball inside of the canon
            //ballRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            ballRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

            // setting the canonBallPrefab to reference our Game's Ball
            cannonBallPrefab = other.gameObject;
        }
    }
}