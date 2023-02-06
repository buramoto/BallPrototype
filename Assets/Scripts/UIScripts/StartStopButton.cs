using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopButton : MonoBehaviour
{   
    public void execute()
    {
        DungeonMaster.dm.changeMode();
    }
}
