using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour
{
    public StateReference.goalColor goalColor;
    // Start is called before the first frame update
    void Start()
    {
        //Initalize goal block with correct color
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (goalColor)
        {
            case StateReference.goalColor.purple:
                //renderer.color = new Color(181, 0, 255);//CHANGE THIS TO PURPLE
                break;
            case StateReference.goalColor.white:
                //renderer.color = Color.white;
                break;
            case StateReference.goalColor.yellow:
                //renderer.color = Color.yellow;
                break;
            case StateReference.goalColor.green:
                // commented this line so that the goal block isnt green in color
                //renderer.color = Color.green;
                break;
            default:
                Debug.Log("Goal color is not set!");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {

            reduceAirForceMagnitude();
        }
    }


    public void reduceAirForceMagnitude()
    {
        // Get a reference to the Area Effector 2D component
        if (GameObject.FindWithTag("Airflow")!=null && GameObject.FindWithTag("WindParticles")) {

            AreaEffector2D areaEffector = GameObject.FindWithTag("Airflow").GetComponent<AreaEffector2D>();
            // Modify the force magnitude
            areaEffector.forceMagnitude = areaEffector.forceMagnitude - 3.5f; // Replace 10.0f with your desired value


            // Get a reference to the Particle System component
            ParticleSystem particleSystem = GameObject.FindWithTag("WindParticles").GetComponent<ParticleSystem>();
            // Get a reference to the Main module
            ParticleSystem.VelocityOverLifetimeModule velocityModule = particleSystem.velocityOverLifetime;
            ParticleSystem.MinMaxCurve speed = velocityModule.speedModifier;
            float newSpeed = speed.constant - 0.1f;
            speed = new ParticleSystem.MinMaxCurve(newSpeed);
            velocityModule.speedModifier = speed;

            int currentCount = Mathf.RoundToInt(particleSystem.emission.GetBurst(0).count.Evaluate(Time.time));
            // Reduce the count by 1
            currentCount--;
            // Create a new burst with the modified count
            ParticleSystem.Burst newBurst = new ParticleSystem.Burst(0.0f, currentCount, 0, 1.0f);
            // Set the modified burst to the Particle System
            ParticleSystem.Burst[] bursts = { newBurst };
            particleSystem.emission.SetBursts(bursts);
        }


    }
}
