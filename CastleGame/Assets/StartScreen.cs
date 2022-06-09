using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject resetter;
    public GameObject pause;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Bar").transform.localScale = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
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
                    if (mouseObject.name == "StartStartBt")
                    {
                        pause.GetComponent<Pause>().gameStarted = true;
                        resetter.GetComponent<StartingSequence>().StartGame();
                    }
                    if (mouseObject.name == "StartExitBt")
                    {
                        Application.Quit(); //Exiting game
                    }
                }

            }
        }



    }
}
