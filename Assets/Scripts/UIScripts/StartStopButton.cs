using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopButton : MonoBehaviour
{

    public TMPro.TextMeshProUGUI text;
    
    private void Start()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        DungeonMaster.dm.StartSim += changeText;

    }

    public void execute()
    {
        if (!DungeonMaster.dm.simulationMode)
        {
            DungeonMaster.dm.simMode(true, StateReference.resetType.start);
            text.text = "Stop";
            UIBehavior.gameUI.disableResetButton();
            UIBehavior.gameUI.changeButtonColor(true);
            //DungeonMaster.dm.instructions.text = "Right Click To Attack Enemy";
        }
        else
        {
            DungeonMaster.dm.simMode(false, StateReference.resetType.ssb);
            text.text = "Start";
            UIBehavior.gameUI.enableResetButton();
            UIBehavior.gameUI.changeButtonColor(false);
            //DungeonMaster.dm.instructions.text = "Use The Tools To The Right To Direct The Ball &\nThen Click Start To Begin Ball's Motion";
        }
    }

    public void changeText(){
        if (!DungeonMaster.dm.simulationMode)
        {   
            text.text = "Stop";
            //DungeonMaster.dm.instructions.text = "Right Click To Attack Enemy";
        }
        else
        {   
            text.text = "Start";
            //DungeonMaster.dm.instructions.text = "Use The Tools To The Right To Direct The Ball &\nThen Click Start To Begin Ball's Motion";
        }
    }
}
