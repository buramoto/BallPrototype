using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    /*
     * This script handles the properties of the heating/cooling element.
     * Its type can be specified in the Unity UI or dynamically with a script
    */
    public bool editable;
    public StateReference.ballMaterial material;
    public bool hasCollided = false;
    public SpriteRenderer elementDisplay;
    public int overLaps;
    public Sprite unhighlightedHeater;
    public Sprite outline;
    //public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        overLaps = 0;
        elementDisplay = GetComponent<SpriteRenderer>();
    }

    public void ChangeTemp(StateReference.temperature temp)
    {
        elementDisplay = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plank") || collision.gameObject.CompareTag("Spring") || collision.gameObject.CompareTag("TempChange"))
        {
            Debug.Log("This is the trigger enter stage in Heater");
            overLaps++;
            Debug.Log("Overlap value becomes" + overLaps);
            if (!DungeonMaster.dm.simulationMode && editable)
            {
                elementDisplay.color = Color.magenta;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Plank") || collision.gameObject.CompareTag("Spring") || collision.gameObject.CompareTag("TempChange"))
        {
            Debug.Log("This is the trigger exit stage in Heater");
            overLaps--;
            Debug.Log("Overlap value becomes" + overLaps);
            elementDisplay.color = Color.white;
        }
    }

    public bool isOverlapping()
    {
        if (overLaps > 0)
            return true;
        return false;
    } 
}
