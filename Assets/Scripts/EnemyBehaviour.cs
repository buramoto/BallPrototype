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
            // Instead of destroying we will SET setActive(FALSE)
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            DungeonMaster.dm.hearts[DungeonMaster.dm.lives - 1].SetActive(false);
            DungeonMaster.dm.lives -= 1;
            Debug.Log("Player has lives left: "+ DungeonMaster.dm.lives);
            Debug.Log("Collided with Player");
            if (DungeonMaster.dm.lives <= 0)
            {
                DungeonMaster.dm.simMode(false, StateReference.resetType.kbe);
                DungeonMaster.dm.lives = 2;
            }

           
            //ResetButton r = FindAnyObjectByType<ResetButton>();
            //r.execute();
            //Need to call the reset function currently or reduce health later

        }
    }
}
