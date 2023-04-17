using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanonManager : MonoBehaviour
{
    // variables for GameObject References
    public GameObject cannonBallPrefab;
    public Transform firePoint;
    public LineRenderer lineRenderer;
    private Camera _cam;
    public GameObject barrelHolder;
    private Collider2D ballCollider2D;
    private Collider2D barrelCollider2D;
    private GameObject ball;
    public static CanonManager canonManager;

    // variables for canon logic
    private const int N_TRAJECTORY_POINTS = 10;
    private bool _pressingMouse = false;
    private Vector3 _initialVelocity;
    public float velocityMultiplier=2f;
    private bool isCanonBallPresent = false;
    public float initialXPosition = 0;
    public float initialYPosition = -2;
    public float canonAutoRotateSpeed = 1;



    private void Awake()
    {
        if (canonManager == null)
        {
            canonManager = this;
        }
    }

    void Start()
    {
        _cam = DungeonMaster.dm.GetComponent<PropPlacer>().mainCam;

        lineRenderer.positionCount = N_TRAJECTORY_POINTS;
        lineRenderer.enabled = false;
        ballCollider2D = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        barrelCollider2D = GameObject.FindWithTag("BarrelHolder").GetComponentInChildren<Collider2D>();
        ball = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (DungeonMaster.dm.simulationMode)
        {
            //Debug.Log("isCannonBallPresent *****" + isCanonBallPresent);
            

            //  one more check to see that if the canon has the ball only then allow the shooting operation.
            if (isCanonBallPresent) {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    _pressingMouse = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    lineRenderer.enabled = false;
                    if (_pressingMouse)
                    {
                        _Fire();
                    }
                    _pressingMouse = false;
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
                    //Debug.Log("ANGLE VALUE: ++++++++" + angle);
                    angle *= -1;

                    if(angle>45)
                    {
                        angle = 45;
                    }
                    else if(angle<-45)
                    {
                        angle = -45;
                    }

                    // set rotation of Cannon Barrel to face mouse position
                    barrelHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    // The ball is within the barrel hence change the all position too
                    cannonBallPrefab.transform.position = GameObject.FindWithTag("CannonBase").transform.position;

                    if (mousePos.y - firePoint.position.y < 0)
                    {
                        Debug.Log("In if");
                        _initialVelocity = (mousePos - firePoint.position) * velocityMultiplier;
                        _initialVelocity.y = 0.707f * firePoint.position.y;
                        lineRenderer.enabled = false;
                    }
                    else
                    {
                        Debug.Log("In else");
                        _initialVelocity = (mousePos - firePoint.position) * velocityMultiplier;
                        lineRenderer.enabled = true;
                    }
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
        cannonBallPrefab.GetComponent<SpriteRenderer>().enabled = true;

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
            other.GetComponent<SpriteRenderer>().enabled = false;

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

    public void barrelCollisionEncountered()
    {
        // here we are checking that if the ball is already in the cannon then don't execute this code
        // but if the ball is not in the cannon then check if the ball is colliding with the BarrelHolder
        // according to the current scenario the BarrelHolder has a separate collider from the cannon object
        // hence we need to do this checking

        Debug.Log("Collision detected with BARREL");
        this.isCanonBallPresent = true;

        ball.transform.position = GameObject.FindWithTag("CannonBase").transform.position;

        // disabling the gravity forces on the ball
        Rigidbody2D ballRigidBody = ball.GetComponent<Rigidbody2D>();
        ballRigidBody.isKinematic = true;
        ball.GetComponent<SpriteRenderer>().enabled = false;

        // Stopping the Ball's Rotation because the ball inside of the canon
        //ballRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        ballRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

        // setting the canonBallPrefab to reference our Game's Ball
        cannonBallPrefab = ball.gameObject;
            
        
    }
}