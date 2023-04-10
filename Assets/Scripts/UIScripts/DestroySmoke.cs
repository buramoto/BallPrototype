using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySmoke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyGameObject", 900 / 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyGameObject()
    {
        Debug.Log("Destroyed The Smoke");
        UIBehavior.gameUI.displayGameOverScreen();
        Destroy(gameObject);
    }
}
