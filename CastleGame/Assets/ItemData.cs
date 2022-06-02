using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    //The item ID (the list of them stored in ItemSpawn). Input
    public int itemTypeID = -1;

    // Start is called before the first frame update
    void Start()
    {
        //Changing item texture to correct one
        Sprite sp = GameObject.FindWithTag("ItemSpawn").GetComponent<Items>().itemSprites[itemTypeID];
        gameObject.GetComponent<SpriteRenderer>().sprite = sp;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
