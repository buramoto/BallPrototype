using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationsBehavior : MonoBehaviour
{
    // public void rotateLeft()
    // {
    //     PropPlacer.propPlacer.rotateLeft();
    // }

    // public void rotateRight()
    // {
    //     PropPlacer.propPlacer.rotateRight();
    // }
    public float rotationSpeed = 30f;
    public float longPressDuration = 0.5f;
    public float longPressRotationSpeedMultiplier = 2f;
    // GameObject toolkitInstance = DungeonMaster.dm.highlightedObject;
    private bool isRotating = false;
    // private float rotateDirection = 0f;
    private float pressStartTime;
    private string direction;
    private IEnumerator toggleRotationCoroutine;

    public void RotateLeft()
    {
        // rotateDirection = -1f;
        isRotating = true;
        pressStartTime = Time.time;
        Debug.Log("Rotate Left");
        // Start coroutine to toggle rotation speed
        toggleRotationCoroutine = ToggleRotationSpeed("left");
        StartCoroutine(toggleRotationCoroutine);
    }

    public void RotateRight()
    {
        // rotateDirection = 1f;
        isRotating = true;
        pressStartTime = Time.time;
        Debug.Log("Rotate Right");
        // Start coroutine to toggle rotation speed
        toggleRotationCoroutine = ToggleRotationSpeed("right");
        StartCoroutine(toggleRotationCoroutine);
    }

    public void EndRotation()
    {
        isRotating = false;
        // rotateDirection = 0f;

        // Stop coroutine
        StopCoroutine(toggleRotationCoroutine);
    }

    private IEnumerator ToggleRotationSpeed(string direction)
    {
        while (isRotating)
        {   
            Debug.Log("Rotating");
            if (Time.time - pressStartTime > longPressDuration)
            {
                Debug.Log("Long Press");
                rotationSpeed = 1000;
            }
            else
            {
                Debug.Log("Short Press");
                rotationSpeed = 500;
            }
            if(direction == "left")
            {
                Debug.Log("Rotating Left"+  rotationSpeed);
                PropPlacer.propPlacer.rotateLeft(rotationSpeed);
            }
            else if(direction == "right")
            {
                Debug.Log("Rotating Right"+  rotationSpeed);
                PropPlacer.propPlacer.rotateRight(rotationSpeed);
            }
            yield return null;
        }

        // Reset rotation speed to default value
        rotationSpeed = 30f;
    }

    public void deleteItem()
    {
        PropPlacer.propPlacer.deleteToolkitInstance();
    }
}
