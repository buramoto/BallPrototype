using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a data class to be used for referencing the states of certain items, such as temperature
/// </summary>
public class StateReference
{
    /// <summary>
    /// This enum holds the references to the different temperature types
    /// </summary>
    public enum temperature
    {
        neutral,
        hot,
        cold
    }

    /// <summary>
    /// This enum is the different colored goal blocks in the scene
    /// </summary>
    public enum goalColor
    {
        purple,
        yellow,
        blue,
        white,
        green
    }
}
