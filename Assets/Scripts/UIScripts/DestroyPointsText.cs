using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPointsText : MonoBehaviour
{
    // Start is called before the first frame update
    //private float timeExisted = 0;
    void Start()
    {
        Invoke("DestroyGameObject", 900/1000f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyGameObject()
    {
        Debug.Log("Destroyed The +50 Text");
        Destroy(gameObject);
    }
}
