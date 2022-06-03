using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public GridNode associatedNode;
    public Level level;
    public abstract void Interact();
}
