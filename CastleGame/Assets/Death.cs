using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject resetter;
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
            mc = new Vector3(mc.x, mc.y, -9.01f);
            GameObject.Find("DeathMenu").transform.position = mc;
            GameObject.Find("Player").GetComponent<Player>().isPlaying = false;
            Rigidbody2D rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0f, 0f);

            var cameraSize = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 4.25f;
            GameObject.Find("DeathMenu").transform.localScale = new Vector3(cameraSize, cameraSize, 1f);

        }


        if (!playerData.isPlaying && !playerData.isAlive)
        {
            if (!isAsking && Input.GetKeyDown(KeyCode.Escape) && GameObject.Find("Player").GetComponent<Player>().cheatsTurnedOn)
            {
                isAsking = false;
                GameObject.Find("DeathMenu").transform.position = new Vector3(-250, -100, -100);
                GameObject.Find("DeathExitSure").transform.localScale = new Vector3(0f, 0f, 0f);
                GameObject.Find("DeathMenu").transform.position = new Vector3(-100, -100, -100);
                GameObject.Find("Player").GetComponent<Player>().isPlaying = true;
                GameObject.Find("Player").GetComponent<Player>().Resurrect();
                GameObject.Find("DeathText").GetComponent<TextMesh>().text = "论 论让欣巳!";

            }
            else
            {
                if (isAsking && Input.GetKeyDown(KeyCode.Escape))
                {
                    GameObject.Find("DeathExitSure").transform.localScale = new Vector3(0f, 0f, 0f);
                    isAsking = false;
                }
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
                            isAsking = false;
                            GameObject.Find("DeathExitSure").transform.localScale = new Vector3(0f, 0f, 0f);
                            resetter.GetComponent<StartingSequence>().StartGame();
                            GameObject.Find("DeathText").GetComponent<TextMesh>().text = "论 论让欣巳!";
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

        }

    }

}
