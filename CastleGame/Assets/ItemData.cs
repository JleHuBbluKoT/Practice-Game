using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    //The item ID (the list of them stored in ItemSpawn). Input
    public int ID = -1;
    public float currentSpeed = 0f; //Speed of the item
    public int owner = -2; //-3 - no one (does damage to anyone), -2 - no one (does not damage), -1 - player. Other - enemies
    public bool pickable = true; //Can an item be taken
    public bool hasCollision = false; //Has an item collision


    // Start is called before the first frame update
    void Start()
    {
        //Changing item texture to correct one
        Sprite sp = GameObject.Find("ItemSpawn").GetComponent<Items>().list[ID].sprite;
        if (sp != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sp;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
