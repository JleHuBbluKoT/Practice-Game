using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGenerator : Editor
{
    AbstractDungeonGenerator generator;
    private void Awake()
    {
        generator = (AbstractDungeonGenerator)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }

        if (GUILayout.Button("Clear Dungeon"))
        {
            generator.ClearDungeon();
        }

        if (GUILayout.Button("GetCell"))
        {
            generator.GainData();
        }
    }

}

