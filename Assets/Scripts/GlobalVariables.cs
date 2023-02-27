using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // Total elements generated
    public static int heaterCounter = 0;
    public static int plankCounter = 0;
    public static int springCounter = 0;

    // Total elements actually used
    public static int heaterUsed = 0;
    public static int plankUsed = 0;
    public static int springUsed = 0;

    public static int attemptCounter = 1;
    public static int kbeCounter = 0;
    public static int oobCounter = 0;
    public static int wgoCounter = 0;
    public static int livesLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Plank Counter: " + plankCounter);
    }
}
