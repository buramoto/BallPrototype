using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject RedSplashScreen;
    public GameObject enemy;
    public Vector2 startPosition;
    void Start()
    {
        //RedSplashScreen = GameObject.FindWithTag("HealthLoss");
        //if (UIBehavior.gameUI.RedSplashScreen == null) {
            //UIBehavior.gameUI.RedSplashScreen = GameObject.FindWithTag("HealthLoss"); 
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (RedSplashScreen != null)
        //{
        //    if (RedSplashScreen.GetComponent<Image>().color.a >0) {
        //        var color = RedSplashScreen.GetComponent<Image>().color;
        //        color.a -= 0.01f;
        //        RedSplashScreen.GetComponent<Image>().color = color;
        //    }

        //}
    }

    public void resetEnemy()
    {
        // enemy.transform.position = startPosition;
        enemy.SetActive(true);
        if(enemy.GetComponent<Rigidbody2D>() != null) {
            Destroy(enemy.GetComponent<Rigidbody2D>());
        }
        enemy.GetComponent<Animation>().enabled = true;
        enemy.GetComponent<Collider2D>().enabled = true;
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
           // Instantiate(DungeonMaster.dm.awardPoints, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
           // Debug.Log("+50 Text Instantiated!!");
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            // DungeonMaster.dm.hearts[DungeonMaster.dm.lives - 1].gameObject.SetActive(false);
            // DungeonMaster.dm.lives -= 1;
            // Debug.Log("Player has lives left: "+ DungeonMaster.dm.lives);
            Debug.Log("Collided with Player");
            //kill enemy when the ball is hot
            BallScript bs = collision.gameObject.GetComponent<BallScript>();
            if(bs.tempState==StateReference.temperature.hot){
                gameObject.SetActive(false);
            }
            else{
                DungeonMaster.dm._ballHealth.DamageUnit(10);
                if (DungeonMaster.dm._ballHealth.Health >= 0)
                {
                    UIBehavior.gameUI.setRedSplashScreen();
                }
            }
            UIBehavior.gameUI.setHealth(DungeonMaster.dm._ballHealth.Health);
            Debug.Log("Player Health: " + DungeonMaster.dm._ballHealth.Health);
            //var color = RedSplashScreen.GetComponent<Image>().color;
            //color.a = 0.9f;
            //RedSplashScreen.GetComponent<Image>().color = color;
            // if (DungeonMaster.dm._ballHealth.Health >= 0)
            // {
            //     // Debug.Log("Current Lives: " + DungeonMaster.dm.lives);
            //     UIBehavior.gameUI.setRedSplashScreen();

            //     //var color = UIBehavior.gameUI.RedSplashScreen.GetComponent<Image>().color;
            //     //color.a = 0.9f;
            //     //UIBehavior.gameUI.RedSplashScreen.GetComponent<Image>().color = color;
            // }
            if(DungeonMaster.dm._ballHealth.Health <= 0) {
                SendToGoogle.sendToGoogle.resetGlobalVariables("KBE");
                Time.timeScale = 0;
                gameObject.SetActive(false);
                UIBehavior.gameUI.displayGameOverScreen();
            }
            // if (DungeonMaster.dm.lives <= 0)
            // {
            //     //var col = RedSplashScreen.GetComponent<Image>().color;
            //     //col.a = 0.00f;
            //     //RedSplashScreen.GetComponent<Image>().color = col;
            //     SendToGoogle.sendToGoogle.resetGlobalVariables("KBE");

            //     // Destroy(gameObject);
            //     gameObject.SetActive(false);
            //     // not wokring as BALLS[] variable is private,
            //     // so either make it public OR find ball by tag OR call the function dm.freezeBall();
            //     // DungeonMaster.dm.freezeBall(0);

            //     //DungeonMaster.dm.balls[0].gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            //     //DungeonMaster.dm.simMode(false, StateReference.resetType.kbe);
            //     // if(RedSplashScreen != null){
            //     //     RedSplashScreen
            //     // }

            //     Time.timeScale = 0;
            //     UIBehavior.gameUI.displayGameOverScreen();

            // }
            
           
            //ResetButton r = FindAnyObjectByType<ResetButton>();
            //r.execute();
            //Need to call the reset function currently or reduce health later

        }
    }
}
