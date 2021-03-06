using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkObject : Interactible
{
    public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ApplyDamage(float Damage)
    {
        spawnItems();
    }
    public override void Interact()
    {
        spawnItems();
    }

    public void spawnItems()
    {
        for (int i = 0; i < UnityEngine.Random.Range(1, 5); i++)
        {
            var randItemID = UnityEngine.Random.Range(0, 5);
            if (randItemID == 2)
            {
                randItemID = 6;
            }
            GameObject.FindWithTag("ItemSpawn").GetComponent<Items>().CreateItem(this.transform.position, randItemID, -2, UnityEngine.Random.Range(0f, 1f), new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0));
        }
        associatedNode.walkable = true;
        Destroy(gameObject);
    }

}
