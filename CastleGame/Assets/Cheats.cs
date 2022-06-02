using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    //Cheats (health, items)
    public bool turnedOn = false; //Are cheats turned on

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (turnedOn)
        {
            //Generating a random item by right clicking mouse on the screen
            if (Input.GetMouseButtonUp(1))
            {
                GameObject.FindWithTag("ItemSpawn").GetComponent<Items>().CreateItem(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);
            }

            //Health cheat (Key I): damage
            if (Input.GetKey("i") && !Health.isDead)
            {
                GameObject.FindWithTag("Player").GetComponent<Health>().ChangeHealth(-0.1f);
            }

            //Health cheat (Key O): heal
            if (Input.GetKey("o") && !Health.isDead)
            {
                GameObject.FindWithTag("Player").GetComponent<Health>().ChangeHealth(0.1f);
            }

            // Recurrect cheat (Key P)
            if (Input.GetKey("p"))
            {
                GameObject.FindWithTag("Player").GetComponent<Health>().Resurrect();
            }

        }
    }
}
