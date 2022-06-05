using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingSequence : MonoBehaviour
{
    public Level level;
    public GameObject square;
    public Items items;
    public GameObject pause;
    public GameObject death;
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        
        pause.transform.position = new Vector3(-100, -100, -9);
        death.transform.position = new Vector3(-250, -100, -9);

        level.ghost.GetComponent<GhostBehaviour>().Restart();
        level.ghost.SetActive(false);
        //square.SetActive(false);

        items.ItemsKill();

        level.ClearDungeon();
        level.CreateGrid();


        GridNode tempNode = level.levelRoomList[0].QuickFreeSpot();
        Vector3 b = level.GridToWorld(new Vector2Int(tempNode.gridX, tempNode.gridY));

        square.transform.position = new Vector3(b.x, b.y, 0);

        square.GetComponent<Player>().Resurrect();
        square.GetComponent<Player>().DefaultInventory();

        level.ghost.SetActive(true);

    }
    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
