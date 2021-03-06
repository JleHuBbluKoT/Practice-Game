using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    //The item ID (the list of them stored in ItemSpawn). Input
    public int ID = -1;
    public Vector3 direction = new Vector3();
    public float currentSpeed = 0f; //Speed of the item
    public int owner = -2; //-3 - no one (does damage to anyone), -2 - no one (does not damage), -1 - player. Other - enemies
    public float force = 1f; //Value between 0f and 1f of original force
    public bool pickable = true; //Can an item be taken
    public bool hasCollision = true; //Has an item collision



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
        var isPlaying = GameObject.Find("Player").GetComponent<Player>().isPlaying;
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        if (isPlaying) {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        if (currentSpeed != 0f && isPlaying)
        {
            var itemsData = GameObject.Find("ItemSpawn").GetComponent<Items>();
            var nextLocationDistance = currentSpeed / GameObject.Find("ItemSpawn").GetComponent<Items>().list[ID].weight
                * force * GameObject.Find("ItemSpawn").GetComponent<Items>().baseSlow;
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + new Vector2(direction.x, direction.y),
                new Vector2(direction.x, direction.y), 1f);

            if (hit.collider != null && (hit.collider.tag != "Button") && (hit.collider.tag != "Item") && (hit.collider.tag != "SlotItem") && hit.collider.tag != "HoldItem" && hit.collider.tag != "Slot" && hit.collider.tag != "Warning" && hit.collider.tag != "MainCamera" && hit.collider.tag != "Bar")
            {
                if (hasCollision)
                {
                    currentSpeed = 0f;
                }

                //Debug.Log("Hit " + hit.collider.tag);

                if (hit.collider.tag == "ExitDoor" && ID == 2)
                {
                    Destroy(hit.collider.gameObject);
                    Destroy(this.gameObject);
                }

                if (hit.collider.tag == "Creature" && (owner == -1 || owner == -3) && itemsData.list[ID].category == "weapon")
                {

                    hit.collider.GetComponent<GhostBehaviour>().StopGhost(itemsData.list[ID].damage * 30f);
                    if (ID != 2)
                    {
                        Destroy(this.gameObject);
                    }

                    //if (!hit.collider.GetComponent<Creature>().Invincible)
                    //{
                    //    hit.collider.GetComponent<Creature>().health -= itemsData.list[ID].damage * 100;
                    //    if (hit.collider.GetComponent<Creature>().health > hit.collider.GetComponent<Creature>().MaxHealth)
                    //    {
                    //        hit.collider.GetComponent<Creature>().health = hit.collider.GetComponent<Creature>().MaxHealth;
                    //    }
                    //    if (hit.collider.GetComponent<Creature>().health <= 0)
                    //    {
                    //        Destroy(hit.transform.gameObject);
                    //    }
                    //}
                    //else
                    //{
                    //    hit.collider.GetComponent<Creature>().stopTime += itemsData.list[ID].damage * 100;
                    //    Destroy(this.gameObject);
                    //}

                }

            }


            transform.position += nextLocationDistance * direction;


            var speedDecrease = GameObject.Find("ItemSpawn").GetComponent<Items>().speedDecrease;
            if (currentSpeed > 0f)
            {
                currentSpeed -= speedDecrease;
                if (currentSpeed < 0f)
                {
                    currentSpeed = 0f;
                }
            }
        }
    }

}
 