using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    //Item class
    [System.Serializable]
    public class Item
    {
        public string name;
        public string category;
        public float weight;
        public float damage;
        public Sprite sprite;
    }

    //Input data
    public GameObject itemObject; //Item common object
    public int lastID = 0; //Will be an ID for an item
    public float speedDecrease = 0.1f;
    public float baseSlow = 0.1f;
    public List<Item> list = new List<Item>(); //Items list
    public static Vector3 defaultDir = new Vector3(0f, 1f, 0f);
    public List<GameObject> spawnedItems = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Function which creates items
    public void CreateItem(Vector3 coordinates, int ID, int owner = -2, float currentSpeed = 0f, Vector3 direction = new Vector3(),
        float force = 1f, bool pickable = true, bool hasCollision = true)
    {

        if (Mathf.Abs(direction[0]) + Mathf.Abs(direction[1]) == 0f)
        {
            direction = new Vector3(0f, 1f, 0f);
        }

        /*items.GetComponent<SpriteRenderer>().sprite = itemList[Random.Range(0, itemList.Count - 1)];*/
        //Copying object
        var obj = Instantiate(itemObject, new Vector3(coordinates[0], coordinates[1], -1f), Quaternion.identity);
        spawnedItems.Add(obj);

        //Saving in ItemSpawn object
        var itemSpawn = GameObject.Find("ItemSpawn").transform;
        obj.transform.parent = itemSpawn;

        //Naming item after ID
        obj.name = "Item" + lastID.ToString();
        lastID++;

        if (ID == -1)
        {
            obj.GetComponent<ItemData>().ID = Random.Range(0, list.Count); //Random item
        }
        else
        {
            obj.GetComponent<ItemData>().ID = ID; //Item by ID
        }

        obj.GetComponent<ItemData>().direction = direction;
        obj.GetComponent<ItemData>().currentSpeed = currentSpeed;
        obj.GetComponent<ItemData>().owner = owner;
        obj.GetComponent<ItemData>().direction = direction;
        obj.GetComponent<ItemData>().force = force;
        obj.GetComponent<ItemData>().pickable = pickable;
        obj.GetComponent<ItemData>().hasCollision = hasCollision;
    }

    public void ItemsKill()
    {
        foreach (var item in spawnedItems)
        {
            Destroy(item);
        }
    }
}
