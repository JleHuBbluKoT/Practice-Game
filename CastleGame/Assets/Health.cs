using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //Input data
    public float maxHealth = 100f; //Max player health
    public float currentHealth = 100f; //Current player health

    //Is player dead check
    public static bool isDead = false;

    //Player healthpoints percentage
    public static float percent = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        //Health bar update if player is not dead
        if (!Health.isDead)
        {
            //Health maximum check
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            //Percantage of current health compared to maximum one
            if (currentHealth != 0f)
            {
                Health.percent = currentHealth / maxHealth;
            }
            else
            {
                Health.percent = 0f;
            }
            
            //Resizing red part of the health bar
            GameObject.FindWithTag("Health").transform.localScale = new Vector3(Health.percent, 1f, 1f);
            GameObject.FindWithTag("Health").transform.position = new Vector3(GameObject.FindWithTag("Bar").transform.position.x +
                Health.percent * GameObject.FindWithTag("Bar").transform.localScale.x / 2 - GameObject.FindWithTag("Bar").transform.localScale.x / 2,
                GameObject.FindWithTag("Bar").transform.position.y, GameObject.FindWithTag("Health").transform.position.z);

        }

        //Applying death due 0 or less healthpoints
        if (currentHealth <= 0f && !Health.isDead)
        {
            currentHealth = 0f;
            Health.isDead = true;
            GameObject.FindWithTag("Health").transform.localScale = new Vector3(0f, 1f, 1f);
            Debug.Log("YOU DIED");
        }

    }

    //Change healthpoints amount
    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
    }

    //Resurrect
    public void Resurrect()
    {
        currentHealth = maxHealth;
        Health.isDead = false;
    }

}
