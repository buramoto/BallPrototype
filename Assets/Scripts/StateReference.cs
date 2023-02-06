using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a data class to be used for referencing the states of certain items, such as temperature
/// </summary>
public class StateReference
{
    public enum temperature
    {
        neutral,
        hot,
        cold
    }
}
