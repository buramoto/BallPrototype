using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject RedSplashScreen;
    void Start()
    {
        RedSplashScreen = GameObject.FindWithTag("HealthLoss");
    }

    // Update is called once per frame
    void Update()
    {
        if (RedSplashScreen != null)
        {
            if (RedSplashScreen.GetComponent<Image>().color.a >0) {
                var color = RedSplashScreen.GetComponent<Image>().color;
                color.a -= 0.01f;
                RedSplashScreen.GetComponent<Image>().color = color;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Sword"))
        {
            Debug.Log("Collided with Sword");
            GlobalVariables.levelScore += 50;
            GameObject scoreText = GameObject.Find("Score_Text");
            Debug.Log("This is the score go" + scoreText);
            if (scoreText != null)
            {
                scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = GlobalVariables.levelScore.ToString();
            }
            // Instead of destroying we will SET setActive(FALSE)
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            DungeonMaster.dm.hearts[DungeonMaster.dm.lives - 1].gameObject.SetActive(false);
            DungeonMaster.dm.lives -= 1;
            Debug.Log("Player has lives left: "+ DungeonMaster.dm.lives);
            Debug.Log("Collided with Player");
            if (DungeonMaster.dm.lives <= 0)
            {
                SendToGoogle.sendToGoogle.resetGlobalVariables("KBE");
                
                // Destroy(gameObject);
                gameObject.SetActive(false);
                // not wokring as BALLS[] variable is private,
                // so either make it public OR find ball by tag OR call the function dm.freezeBall();
                // DungeonMaster.dm.freezeBall(0);

                //DungeonMaster.dm.balls[0].gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                //DungeonMaster.dm.simMode(false, StateReference.resetType.kbe);
                // if(RedSplashScreen != null){
                //     RedSplashScreen
                // }
                var col = RedSplashScreen.GetComponent<Image>().color;
                col.a = 0.00f;
                RedSplashScreen.GetComponent<Image>().color = col;
                Time.timeScale = 0;
                UIBehavior.gameUI.displayGameOverScreen();

            }
            var color = RedSplashScreen.GetComponent<Image>().color;
            color.a = 0.9f;
            RedSplashScreen.GetComponent<Image>().color = color;
           
            //ResetButton r = FindAnyObjectByType<ResetButton>();
            //r.execute();
            //Need to call the reset function currently or reduce health later

        }
    }
}
