using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateSword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSwordInActive()
    {
        Debug.Log("Sword About to SET ENABLED = FALSE");
        gameObject.SetActive(false);
        //sword = gameObject.GetComponentInChildren<CapsuleCollider2D>();
        //sword.enabled = false;
    }
}
