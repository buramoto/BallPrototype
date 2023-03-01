using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    public void execute()
    {
        DungeonMaster.dm.loadNextLevel(DungeonMaster.dm.nextSceneName);
    }
}
