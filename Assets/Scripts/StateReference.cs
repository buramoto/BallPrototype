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
    /// <summary>
    /// This enum is used to tell the different types of resets, such as out of bounds
    /// </summary>
    public enum resetType
    {
        kbe, // killed by enemy
        oob, //Out of bounds
        ssb, //Start/stop button
        wgo, //wrong goal order
        win, //Completed level
        start //For scripts starting simulation. No message is needed, so this is the "none" type
    }

    public enum ballMaterial
    {
        normal,
        wood,
        steel,
        rubber
    }
}
