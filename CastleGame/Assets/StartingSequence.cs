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
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        pause.transform.position = new Vector3(-100,-100,-9);
        
        level.ghost.SetActive(false);
        level.ghost.GetComponent<GhostBehaviour>().Restart();

        items.ItemsKill();

        level.ClearDungeon();
        level.CreateGrid();
        square.transform.position = new Vector3(79, 79, 0);
        square.GetComponent<Player>().Resurrect();
        square.GetComponent<Player>().DefaultInventory();

        level.ghost.SetActive(true);

    }
    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
