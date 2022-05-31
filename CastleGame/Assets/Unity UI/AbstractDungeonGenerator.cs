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
    public void GainData()
    {
        GetData();
    }
    protected abstract void RunLevelGeneration();
    protected abstract void LevelClear();
    protected abstract void GetData();
}
