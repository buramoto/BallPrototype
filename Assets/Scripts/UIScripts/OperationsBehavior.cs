using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationsBehavior : MonoBehaviour
{
    public void rotateLeft()
    {
        PropPlacer.propPlacer.rotateLeft();
    }

    public void rotateRight()
    {
        PropPlacer.propPlacer.rotateRight();
    }

    public void deleteItem()
    {
        PropPlacer.propPlacer.deleteToolkitInstance();
    }
}
