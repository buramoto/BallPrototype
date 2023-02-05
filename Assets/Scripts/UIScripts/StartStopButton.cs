using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopButton : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void execute()
    {
        DungeonMaster.dm.changeMode();
        if (DungeonMaster.dm.simulatonMode)
        {
            text.text = "Stop";
        }
        else
        {
            text.text = "Start";
        }
    }
}
