using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1 : MonoBehaviour
{
    public static int instructionIndex = 0;
    public GameObject[] instructionArray = new GameObject[5];
    public static Tutorial1 tutorial1Reference;
    // Start is called before the first frame update

    private void Awake()
    {
        if (tutorial1Reference == null)
        {
            tutorial1Reference = this;
        }
        else
        {

        }
    }

    void Start()
    {
        
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
