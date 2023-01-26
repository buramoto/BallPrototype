using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProperties : MonoBehaviour
{
    //State variables
    public enum temperature
    {
        neutral,
        hot,
        cold
    }

    //Outputs
    public GameObject ball;
    public temperature tempState;

    private SpriteRenderer ballDisplay;
    

    // Start is called before the first frame update
    void Start()
    {
        ball.transform.position = new Vector3(0, 0, 0);
        tempState = temperature.neutral;
        ball.SetActive(true);
        ballDisplay = GetComponent<SpriteRenderer>();
        ballDisplay.material.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Plank":
                plankCollision(collision.gameObject);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ChangeTemperature elementProperties = other.gameObject.GetComponent<ChangeTemperature>();
        if(elementProperties.setting == ChangeTemperature.tempType.heater)
        {
            Debug.Log("Collided with heater");
            tempState = temperature.hot;
            ballDisplay.material.color = Color.red;
        }
    }

    private void plankCollision(GameObject plank)
    {
        Debug.Log("Collided with plank");
        //TODO some property check here
        if(tempState == temperature.hot)
        {
            Plank plankProperties = plank.gameObject.GetComponent<Plank>();
            switch(plankProperties.plankState)
            {
                case Plank.plankTemp.ice:
                    tempState = temperature.neutral;
                    ballDisplay.material.color = Color.gray;
                    plank.SetActive(false);
                    break;
                case Plank.plankTemp.fire:
                    break;
                case Plank.plankTemp.normal:
                    break;
            }
        }
        return;
    }
}
