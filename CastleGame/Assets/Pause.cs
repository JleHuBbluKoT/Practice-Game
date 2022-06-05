using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject slotItemObject;
    public Player.Slot holdItem = new Player.Slot();
    public static bool isPlaying;
    public static bool isAsking;
    public static bool isReseting = false;
    public static bool isExiting = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isPlaying = GameObject.Find("Player").GetComponent<Player>().isPlaying;
        var isAlive = GameObject.Find("Player").GetComponent<Player>().isAlive;

        // Pressing escape
        if (Input.GetKeyDown(KeyCode.Escape) && !isAsking && isAlive)
        {
            Toggle();
        }

        if (!isPlaying && isAlive)
        {
            if (isAsking && Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.Find("ResetSure").transform.localScale = new Vector3(0f, 0f, 0f);
                GameObject.Find("ExitSure").transform.localScale = new Vector3(0f, 0f, 0f);
                isAsking = false;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject)
                {
                    //Mouse detecting
                    var mouseObject = targetObject.transform.gameObject;

                    if (mouseObject.tag == "Button" && holdItem.ID == -1)
                    {
                        if (mouseObject.name == "ResumeBt" && !(isExiting || isReseting))
                        {
                            Resume();
                        }
                        if (mouseObject.name == "ResetBt" && !isExiting)
                        {
                            isAsking = true;
                            isReseting = true;
                            GameObject.Find("ResetSure").transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                        if (mouseObject.name == "ResetCancel")
                        {
                            isAsking = false;
                            isReseting = false;
                            GameObject.Find("ResetSure").transform.localScale = new Vector3(0f, 0f, 0f);
                        }
                        if (mouseObject.name == "ResetConfirm")
                        {
                            //Restart
                        }
                        if (mouseObject.name == "ExitBt" && !isReseting)
                        {
                            isAsking = true;
                            isExiting = true;
                            GameObject.Find("ExitSure").transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                        if (mouseObject.name == "ExitCancel")
                        {
                            isAsking = false;
                            isExiting = false;
                            GameObject.Find("ExitSure").transform.localScale = new Vector3(0f, 0f, 0f);
                        }
                        if (mouseObject.name == "ExitConfirm")
                        {
                            Application.Quit(); //Exiting game
                        }
                    }


                    if (mouseObject.tag == "Slot" && !isAsking)
                    {
                        var spl = mouseObject.name.Split(char.Parse("u"));
                        var slotID = int.Parse(spl[1]);
                        //Debug.Log(slotID);

                        var player = GameObject.Find("Player").GetComponent<Player>();
                        if (holdItem.ID == -1)
                        {
                            if (slotID < player.inventory.Count)
                            {
                                holdItem = player.inventory[slotID];
                                player.inventory.RemoveAt(slotID);
                                UpdateInventory();

                                var obj = Instantiate(slotItemObject, new Vector3(mousePosition.x, mousePosition.y, -9.5f), Quaternion.identity);
                                obj.transform.parent = GameObject.Find("HoldMenu").transform;
                                obj.transform.localScale = new Vector3(1f, 1f, 1f);
                                obj.name = "HoldItem";

                                var itemList = GameObject.Find("ItemSpawn").GetComponent<Items>().list;
                                if (itemList[holdItem.ID].sprite != null)
                                {
                                    obj.transform.Find("SlotItemSprite").GetComponent<SpriteRenderer>().sprite = itemList[holdItem.ID].sprite;
                                }
                                obj.transform.Find("SlotItemText").GetComponent<TextMesh>().text = holdItem.amount.ToString();
                            }
                        }
                        else
                        {
                            if (slotID > player.inventory.Count)
                            {
                                slotID = player.inventory.Count;
                            }
                            player.inventory.Insert(slotID, holdItem);
                            UpdateInventory();
                            holdItem = new Player.Slot();
                            Destroy(GameObject.Find("HoldItem"));
                        }
                    }


                }
            }

            if ((Input.GetKeyDown("1") || Input.GetKeyDown("2") || Input.GetKeyDown("3") || Input.GetKeyDown("4") || Input.GetKeyDown("5")) && !isAsking)
            {
                int keyPressed = -1;
                if (Input.GetKeyDown("5"))
                {
                    keyPressed = 4;
                }
                if (Input.GetKeyDown("4"))
                {
                    keyPressed = 3;
                }
                if (Input.GetKeyDown("3"))
                {
                    keyPressed = 2;
                }
                if (Input.GetKeyDown("2"))
                {
                    keyPressed = 1;
                }
                if (Input.GetKeyDown("1"))
                {
                    keyPressed = 0;
                }
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject)
                {
                    //Mouse detecting
                    var mouseObject = targetObject.transform.gameObject;

                    if (mouseObject.tag == "Slot")
                    {
                        var spl = mouseObject.name.Split(char.Parse("u"));
                        var slotID = int.Parse(spl[1]);
                        //Debug.Log(slotID);

                        var player = GameObject.Find("Player").GetComponent<Player>();
                        if (holdItem.ID == -1)
                        {
                            if (slotID < player.inventory.Count)
                            {
                                holdItem = player.inventory[slotID];
                                player.inventory[slotID] = player.inventory[keyPressed];
                                player.inventory[keyPressed] = holdItem;
                                holdItem = new Player.Slot();
                                UpdateInventory();
                            }
                        }
                    }


                }
            }
        }

    }

    //Toggle play and pause mods
    public void Toggle()
    {
        if (isPlaying)
        {
            Stop();
        } 
        else
        {
            if (holdItem.ID == -1)
            {
                Resume();
            }
        }
    }

    //Pause playing
    public void Stop()
    {
        var mc = GameObject.FindWithTag("MainCamera").transform.position;
        mc = new Vector3(mc.x, mc.y, -9f);
        GameObject.Find("PauseMenu").transform.position = mc;
        GameObject.Find("Player").GetComponent<Player>().isPlaying = false;
        Rigidbody2D rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 0f);

        var cameraSize = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 4.25f;
        GameObject.Find("PauseMenu").transform.localScale = new Vector3(cameraSize, cameraSize, 1f);

        UpdateInventory();

        GameObject.Find("MainSlots").transform.position += new Vector3(0f, 0f, 100f);

        //GameObject.Find("SlotMenuSelected").transform.localPosition = new Vector3((GameObject.Find("Player").GetComponent<Player>().activeSlot - 2f) / 5f, 0.2f, -0.5f);

    }

    //Resume playing
    public void Resume()
    {
        GameObject.Find("PauseMenu").transform.position = new Vector3(-100, -100, -100);
        GameObject.Find("Player").GetComponent<Player>().isPlaying = true;

        GameObject.Find("MainSlots").transform.position += new Vector3(0f, 0f, -100f);
    }

    //Updates inventory
    public void UpdateInventory()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("SlotItem");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        var playerData = GameObject.Find("Player").GetComponent<Player>();
        if (playerData.isAlive)
        {

            var inventory = playerData.inventory;

            var maxSlots = 15;
            if (playerData.inventory.Count < 15)
            {
                maxSlots = playerData.inventory.Count;
            }

            var itemList = GameObject.Find("ItemSpawn").GetComponent<Items>().list;

            for (int i1 = 0; i1 < maxSlots; i1++)
            {
                var obj = Instantiate(slotItemObject, GameObject.Find("SlotMenu" + i1.ToString()).transform.position, Quaternion.identity);
                obj.transform.parent = GameObject.Find("SlotMenu" + i1.ToString()).transform;
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
                obj.name = "SlotItem" + i1.ToString();
                
                if (itemList[inventory[i1].ID].sprite != null)
                {
                    obj.transform.Find("SlotItemSprite").GetComponent<SpriteRenderer>().sprite = itemList[inventory[i1].ID].sprite;
                }
                obj.transform.Find("SlotItemText").GetComponent<TextMesh>().text = inventory[i1].amount.ToString();
            }

        }
    }
}
