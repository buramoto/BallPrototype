using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Sword"))
        {
            Debug.Log("Collided with Sword");
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");
            ResetButton r = FindAnyObjectByType<ResetButton>();
            r.execute();
            //Need to call the reset function currently or reduce health later

        }
    }
}
