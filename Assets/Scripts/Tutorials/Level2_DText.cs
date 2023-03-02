using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2_DText : MonoBehaviour
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
        Debug.Log("We are getting collision");
        if (collision.gameObject.name=="Ball" && this.name=="Collider1")
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Use it to guide the ball";
        }
        else if(collision.gameObject.name == "Ball" && this.name == "Collider2")
        {
            GameObject dtext = GameObject.Find("Level_Text");
            Debug.Log(dtext);
            dtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Use it to your advantage";
        }
    }
}
