using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectLib : MonoBehaviour
{
    public GameObject interactiblesLayer;
    public GameObject junk;
    public GameObject door;
    public GameObject keyStorage;
    public GameObject keyDoor;



    public void PlaceKeyDoor(Vector2Int keyPos, Level level)
    {
        GameObject e = Instantiate(keyDoor);
        e.transform.parent = interactiblesLayer.transform;
        e.transform.position = new Vector3((keyPos.x + 0.5f) * level.library.globalGrid.transform.localScale.x, (keyPos.y + 0.5f) * level.library.globalGrid.transform.localScale.y, 0);
        e.transform.localScale = new Vector3(level.library.globalGrid.transform.localScale.x, level.library.globalGrid.transform.localScale.y, level.library.globalGrid.transform.localScale.z);
        associate(new Vector2Int(keyPos.x, keyPos.y), level, e);
        level.grid[keyPos.x, keyPos.y].walkable = false;
    }

    public void PlaceKeyStorage(Vector2Int keyPos, Level level)
    {
        GameObject e = Instantiate(keyStorage);
        e.transform.parent = interactiblesLayer.transform;
        e.transform.position = new Vector3((keyPos.x + 0.5f) * level.library.globalGrid.transform.localScale.x, (keyPos.y + 0.5f) * level.library.globalGrid.transform.localScale.y, 0);
        e.transform.localScale = new Vector3(level.library.globalGrid.transform.localScale.x, level.library.globalGrid.transform.localScale.y, level.library.globalGrid.transform.localScale.z);
        associate(new Vector2Int(keyPos.x, keyPos.y), level, e);
        level.grid[keyPos.x, keyPos.y].walkable = false;
    }

    public void PlaceJunk(Vector2Int junkPos, Level level)
    {
        GameObject e = Instantiate(junk);
        e.transform.parent = interactiblesLayer.transform;
        e.transform.position = new Vector3((junkPos.x + 0.5f) * level.library.globalGrid.transform.localScale.x, (junkPos.y + 0.5f) * level.library.globalGrid.transform.localScale.y, 0);
        e.transform.localScale = new Vector3(level.library.globalGrid.transform.localScale.x, level.library.globalGrid.transform.localScale.y, level.library.globalGrid.transform.localScale.z);
        associate(new Vector2Int(junkPos.x, junkPos.y), level, e);
        level.grid[junkPos.x, junkPos.y].walkable = false;
    }

    public void PlaceDoor( Vector2Int dorPos, bool horizontal, Level level ) {
        GameObject e = Instantiate(door);
        e.GetComponent<DoorObject>().horizontal = horizontal;
        e.transform.parent = interactiblesLayer.transform;
        e.transform.position = new Vector3((dorPos.x + 0.5f) * level.library.globalGrid.transform.localScale.x, (dorPos.y + 0.5f) * level.library.globalGrid.transform.localScale.y, 0);
        e.transform.localScale = new Vector3(level.library.globalGrid.transform.localScale.x / 2, level.library.globalGrid.transform.localScale.y / 2, level.library.globalGrid.transform.localScale.z);
        associate(new Vector2Int(dorPos.x, dorPos.y), level, e);
    }
    public void associate(Vector2Int coords, Level level, GameObject interactible) { //сюда ввод€тс€ координаты относительно сетки
        level.grid[coords.x, coords.y].associatedObject = interactible;
        level.levelInteractibles.Add(interactible);
        interactible.GetComponent<Interactible>().associatedNode = level.grid[coords.x, coords.y];
    }
    
    public void remove(GameObject interactible) {
        interactible.GetComponent<Interactible>().associatedNode.associatedObject = null;
        //interactible.GetComponent<Interactible>().level.levelInteractibles.Remove(interactible);
        Destroy(interactible);
    }
    public void remove(Vector2Int coords, Level level) {
        GameObject interactible = level.grid[coords.x, coords.y].associatedObject;
        interactible.GetComponent<Interactible>().associatedNode.associatedObject = null;
        //interactible.GetComponent<Interactible>().level.levelInteractibles.Remove(interactible);
        Destroy(interactible); 
    }
}
