using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyStorage : Interactible
{
    public GameObject item;

    void Start()
    {
    }
    void Update()
    {

    }

    public override void ApplyDamage(float Damage)
    {
        spawnItems();
    }
    public override void Interact()
    {
        //spawnItems();
    }

    public void spawnItems()
    {
        GameObject.FindWithTag("ItemSpawn").GetComponent<Items>().CreateItem(this.transform.position, 2, -3, UnityEngine.Random.Range(0f, 1f), new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0));
        associatedNode.walkable = true;
        Destroy(gameObject);
    }
}
