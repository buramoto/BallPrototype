using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeButton : MonoBehaviour
{
    /// <summary>
    /// This method is used to invoke the "changeBallSize" method in the DM. Pass in
    /// 0,1,2 for small, med, large resp. for size. Unity UI does not allow for custom
    /// data types to be passed in through the existing button script
    /// </summary>
    /// <param name="size"></param>
    public void changeSize(int size)
    {
        if (DungeonMaster.dm.simulationMode)
        {
            return;
        }
        //DungeonMaster.dm.changeBallSize(size);
        switch (size)
        {
            case 0:
                DungeonMaster.dm.changeBallSize(StateReference.ballType.small);
                break;
            case 1:
                DungeonMaster.dm.changeBallSize(StateReference.ballType.medium);
                break;
            case 2:
                DungeonMaster.dm.changeBallSize(StateReference.ballType.large);
                break;
        }
    }
}
