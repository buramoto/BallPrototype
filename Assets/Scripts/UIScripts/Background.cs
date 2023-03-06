using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background bg;

    /// <summary>
    /// Used to handle background behavior. Future scope: change backround based on level
    /// </summary>
    void Awake()
    {
        if(bg == null)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
