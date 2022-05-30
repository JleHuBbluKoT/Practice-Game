using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    public void GenerateDungeon()
    {
        //tilemapVisualiser.Clear();
        RunLevelGeneration();
    }
    public void ClearDungeon()
    {
        LevelClear();
    }
    protected abstract void RunLevelGeneration();
    protected abstract void LevelClear();
}
