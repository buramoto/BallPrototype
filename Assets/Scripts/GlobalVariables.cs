using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateReference;

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

    public static List<GameObject> usedHeaterObjects = new List<GameObject>();
    public static List<GameObject> usedPlankObjects = new List<GameObject>();
    public static List<GameObject> usedSpringObjects = new List<GameObject>();

    public static int levelScore = 0;

    public static int starsCounter = 0;

    /// <summary>
    /// Dictionary to store corresponding color and points to be awarded
    /// </summary>
    public static Dictionary<goalColor, int> scoreDict = new Dictionary<goalColor, int> { [goalColor.purple] = 50, [goalColor.yellow] = 50, [goalColor.white] = 50 };


    // Level Name
    public static string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
