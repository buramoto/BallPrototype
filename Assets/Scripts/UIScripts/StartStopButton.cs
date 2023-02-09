using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopButton : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void execute()
    {
        if (!DungeonMaster.dm.simulationMode)
        {
            DungeonMaster.dm.simMode(true, StateReference.resetType.start);
            text.text = "Stop";
        }
        else
        {
            DungeonMaster.dm.simMode(false, StateReference.resetType.ssb);
            text.text = "Start";
        }
    }
}
