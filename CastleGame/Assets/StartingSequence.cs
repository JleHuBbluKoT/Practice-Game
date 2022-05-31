using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSequence : MonoBehaviour
{
    public Level level;
    public GameObject square;
    void Start()
    {
        level.CreateGrid();
    }

}
