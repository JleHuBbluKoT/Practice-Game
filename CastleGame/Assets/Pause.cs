using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool isPlaying;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isPlaying = player.GetComponent<Player>().isPlaying;

        // Pressing escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    //Toggle play and pause mods
    public void Toggle()
    {
        if (isPlaying)
        {
            Stop();
        } else
        {
            Resume();
        }
    }

    //Pause playing
    public void Stop()
    {
        var mc = GameObject.FindWithTag("MainCamera").transform.position;
        mc = new Vector3(mc.x, mc.y, -9f);
        GameObject.Find("PauseMenu").transform.position = mc;
        GameObject.Find("Player").GetComponent<Player>().isPlaying = false;
    }

    //Resume playing
    public void Resume()
    {
        GameObject.Find("PauseMenu").transform.position = new Vector3(-100, -100, -100);
        GameObject.Find("Player").GetComponent<Player>().isPlaying = true;
    }

}
