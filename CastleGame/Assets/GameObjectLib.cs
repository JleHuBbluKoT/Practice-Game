using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectLib : MonoBehaviour
{
    public GameObject door;


    public void PlaceDoor( Vector2Int dorPos, bool horizontal, Level level ) {
        GameObject e = Instantiate(door);
        e.GetComponent<DoorObject>().horizontal = horizontal;
        e.transform.position = new Vector3((dorPos.x + 0.5f) * level.library.globalGrid.transform.localScale.x, (dorPos.y + 0.5f) * level.library.globalGrid.transform.localScale.y, 0);
        e.transform.localScale = new Vector3(level.library.globalGrid.transform.localScale.x / 2, level.library.globalGrid.transform.localScale.y / 2, level.library.globalGrid.transform.localScale.z);
        associate(new Vector2Int(dorPos.x, dorPos.y), level, e);
    }
    public void associate(Vector2Int coords, Level level, GameObject interactible) { //сюда ввод€тс€ координаты относительно сетки
        level.grid[coords.x, coords.y].associatedObject = interactible;
        level.levelInteractibles.Add(interactible);
    }
    /*
    public void associate(Vector2Int coords, Level level, GameObject interactible)
    { //сюда ввод€тс€ координаты относительно сетки
        level.grid[coords.x, coords.y].associatedObject = interactible;
        level.levelInteractibles.Add(interactible);
    }*/
}
