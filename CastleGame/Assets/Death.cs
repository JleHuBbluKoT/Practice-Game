using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public static bool isPlaying;
    public static bool isAsking = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var playerData = GameObject.Find("Player").GetComponent<Player>();
        // Pressing escape
        if (!playerData.isAlive)
        {
            var mc = GameObject.FindWithTag("MainCamera").transform.position;
            mc = new Vector3(mc.x, mc.y, -9f);
            GameObject.Find("DeathMenu").transform.position = mc;
            GameObject.Find("Player").GetComponent<Player>().isPlaying = false;
            Rigidbody2D rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0f, 0f);

            var cameraSize = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 4.25f;
            GameObject.Find("DeathMenu").transform.localScale = new Vector3(cameraSize, cameraSize, 1f);

            GameObject.Find("PauseMenu").transform.position = new Vector3(-100, -100, -100);
        }


        if (!playerData.isPlaying && !playerData.isAlive)
        {
            if (!isAsking && Input.GetKeyDown(KeyCode.Escape) && GameObject.Find("Player").GetComponent<Player>().cheatsTurnedOn)
            {
                GameObject.Find("DeathMenu").transform.position = new Vector3(-250, -100, -100);
                playerData.Resurrect();
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject)
                {
                    //Mouse detecting
                    var mouseObject = targetObject.transform.gameObject;

                    if (mouseObject.tag == "Button")
                    {
                        if (mouseObject.name == "DeathResetBt")
                        {
                            //Resurrect in new level
                            GameObject.Find("DeathMenu").transform.position = new Vector3(-250, -100, -100);
                            Debug.Log("sdgs");
                            playerData.Resurrect();
                        }
                        if (mouseObject.name == "DeathExitBt")
                        {
                            isAsking = true;
                            GameObject.Find("DeathExitSure").transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                        if (mouseObject.name == "DeathExitCancel")
                        {
                            isAsking = false;
                            GameObject.Find("DeathExitSure").transform.localScale = new Vector3(0f, 0f, 0f);
                        }
                        if (mouseObject.name == "DeathExitConfirm")
                        {
                            Application.Quit(); //Exiting game
                        }
                    }

                }
            }

            if (isAsking && Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.Find("DeathExitSure").transform.localScale = new Vector3(0f, 0f, 0f);
                isAsking = false;
            }
        }

    }

}
