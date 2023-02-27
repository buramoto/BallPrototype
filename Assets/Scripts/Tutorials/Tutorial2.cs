using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2 : MonoBehaviour
{

    public static int instructionIndex = 0;
    public GameObject[] instructionArray = new GameObject[5];
    public static Tutorial2 tutorial2Reference;

    // Start is called before the first frame update
    void Start()
    {
        if (tutorial2Reference == null)
        {
            tutorial2Reference = this;
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateInstructionIndex()
    {
        instructionIndex += 1;
    }
}
