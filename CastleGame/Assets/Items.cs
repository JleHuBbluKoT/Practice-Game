using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    //Input data
    public GameObject itemObject; //Item common object
    public int lastID = 0; //Will be an ID for an item
    public List<string> itemNames = new List<string>(); //Item names list
    public List<Sprite> itemSprites = new List<Sprite>(); //Item sprites list


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Function which creates items. 1 - X float, 2 - Y float, item ID int. ID = -1 is random item.
    public void CreateItem(float x, float y, int ID)
    {
        /*items.GetComponent<SpriteRenderer>().sprite = itemList[Random.Range(0, itemList.Count - 1)];*/
        //Copying object
        var obj = Instantiate(itemObject, new Vector3(x, y, 0f), Quaternion.identity);

        //Saving in ItemSpawn object
        var itemSpawn = GameObject.Find("ItemSpawn").transform;
        obj.transform.parent = itemSpawn;

        //Naming item after ID
        obj.name = "Item_" + lastID.ToString(); 
        lastID++;

        if (ID == -1)
        {
            obj.GetComponent<ItemData>().itemTypeID = Random.Range(0, itemNames.Count); //Random item
        }
        else
        {
            obj.GetComponent<ItemData>().itemTypeID = ID; //Item by ID
        }
    }
}
