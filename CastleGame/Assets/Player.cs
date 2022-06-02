using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool CheatsTurnedOn = false;
    //<HEALTH> data
    public float healthMax = 100f; //Max player health points
    public float healthCurrent = 100f; //Current player health points
    public bool isAlive = true; //Is player alive check

    //Player health points percentage
    public static float healthPercent = 1f;
  //</HEALTH> data

  //<HUNGER> data
    public float hungerMax = 100f; //Max player hunger points
    public float hungerCurrent = 100f; //Current player hunger points
    public float hungerRecoverMinPercent = 0.5f; //Minimum of hunger bar percent to start regenerating health
    public float hungerRecoverValue = 0.0025f; //Health recovery, when hunger percent is larger or equal to hungerRecoverMinPercent
    public float hungerDamage = 0.005f; //Damage by being hungry
    public float hungerNaturalDecrease = 0.0005f; //Natural hunger decrease by time

    //Player hunger points percentage
    public static float hungerPercent = 1f;
  //</HUNGER> data

  //<TEMPERATURE> data
    public float temperatureMin = -50f; //Min available player temperature. Less then this value kills the player.
    public float temperatureMax = 50f; //Max available player temperature. More then this value kills the player.
    public float temperatureCurrent = 0f; //Current player temperature
    public float temperatureDamage = 0.02f; //High or low temperature damage

  //Player temerature points percentage
    public static float temperaturePercent = 0f;
  //</TEMPERATURE> data


  //<MOVEMENT> data
    public float speed = 1f; //Controls velocity multiplier
    public Level level;
    public float playerReach = 10f;

    Rigidbody2D rb;
    //</MOVEMENT> data

    // PLAYER DATA
    GridNode lookAtNode;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player    
    }

    // Update is called once per frame
    void Update()
    {

        // Interaction with objects on grid
        if (isAlive)
        {
            float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            lookAtNode = level.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log(lookAtNode.gridX + " " + lookAtNode.gridY + " " + lookAtNode.type + " "+ level.distanceBetweenPoints(transform.position, new Vector2Int(lookAtNode.gridX, lookAtNode.gridY)));
                if (lookAtNode.associatedObject != null)
                {
                    if (level.distanceBetweenPoints(this.transform.position, level.GridToWorld(new Vector2Int(lookAtNode.gridX, lookAtNode.gridY))) <= playerReach)
                    {
                        lookAtNode.associatedObject.GetComponent<Interactible>().Interact();
                    }
                }
            }
        }
        

        //<HEALTH> update
        //Health bar update if player is not dead
        if (isAlive) {
            //Health maximum check
            if (healthCurrent > healthMax) {
                healthCurrent = healthMax;
            }

            //Health minimum check
            if (healthCurrent < 0f)
            {
                healthCurrent = 0f;
            }

            //Percantage of current health compared to the maximum
            healthPercent = healthCurrent / healthMax;

            //Resizing part of the health bar
            GameObject.Find("Health").transform.position = new Vector3(GameObject.Find("Bar").transform.position.x +
                healthPercent * GameObject.Find("Bar").transform.localScale.x / 2 - GameObject.Find("Bar").transform.localScale.x / 2,
                GameObject.Find("Bar").transform.position.y + 1f, GameObject.Find("Health").transform.position.z);
            GameObject.Find("Health").transform.localScale = new Vector3(healthPercent, 1f / 3f, 1f);

        }

        //Applying death due 0 or less healthpoints
        if (healthCurrent <= 0f && isAlive)
        {
            isAlive = false;
            Debug.Log("YOU DIED");
        }

      //</HEALTH> update


      //<HUNGER> update
        if (isAlive)
        {
            //Natural hunger decrease by time
            hungerCurrent -= hungerNaturalDecrease;

            //Hunger maximum check
            if (hungerCurrent > hungerMax)
            {
                hungerCurrent = hungerMax;
            }

            //Hunger minimum check
            if (hungerCurrent < 0f)
            {
                hungerCurrent = 0f;
            }

            //Percantage of current hunger compared to the maximum
            hungerPercent = hungerCurrent / hungerMax;

            //Resizing part of the hunger bar
            GameObject.Find("Hunger").transform.position = new Vector3(GameObject.Find("Bar").transform.position.x +
                hungerPercent * GameObject.Find("Bar").transform.localScale.x / 2 - GameObject.Find("Bar").transform.localScale.x / 2,
                GameObject.Find("Bar").transform.position.y, GameObject.Find("Hunger").transform.position.z);
            GameObject.Find("Hunger").transform.localScale = new Vector3(hungerPercent, 1f / 3f, 1f);

            //Max hunger bar recovers health
            if (isAlive && (hungerPercent >= hungerRecoverMinPercent))
            {
                healthCurrent += hungerRecoverValue;
            }

            //Hunger damage
            if (isAlive && (hungerCurrent <= 0))
            {
                healthCurrent -= hungerDamage;
            }
        }
      //</HUNGER> update

      //<TEMPERATURE> update
        if (isAlive)
        {
            //Temperature minimum check
            if (temperatureCurrent < temperatureMin)
            {
                temperatureCurrent = temperatureMin;
            }

            //Temperature maximum check
            if (temperatureCurrent > temperatureMax)
            {
                temperatureCurrent = temperatureMax;
            }

            //Percantage of current temperature compared to the minimum and the maximum
            temperaturePercent = (temperatureCurrent - temperatureMin) / (temperatureMax - temperatureMin);

            //Resizing part of the temperature bar
            GameObject.Find("Temperature").transform.position = new Vector3(GameObject.Find("Bar").transform.position.x +
                temperaturePercent * GameObject.Find("Bar").transform.localScale.x / 2 - GameObject.Find("Bar").transform.localScale.x / 2,
                GameObject.Find("Bar").transform.position.y - 1f, GameObject.Find("Temperature").transform.position.z);
            GameObject.Find("Temperature").transform.localScale = new Vector3(temperaturePercent, 1f / 3f, 1f);

            //Changing color of the temperature bar
            GameObject.Find("Temperature").GetComponent<SpriteRenderer>().color = new Color(temperaturePercent, 0, 1 - temperaturePercent, 1);


            //Temperature damage
            if (isAlive && temperatureCurrent <= temperatureMin)
            {
                healthCurrent -= temperatureDamage;
            }
            if (isAlive && temperatureCurrent >= temperatureMax)
            {
                healthCurrent -= temperatureDamage;
            }
        }
      //</TEMPERATURE> update
        

      //<MOVEMENT> update
        if (isAlive)
        {
            float xMove = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
            float yMove = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1

            rb.velocity = new Vector2(xMove, yMove) * speed * healthPercent; // Creates velocity in direction of value equal to keypress (WASD). rb.velocity.y deals with falling + jumping by setting velocity to y. Movement speed depends on healthbat percentage.

            Vector2Int v = new Vector2Int(level.WorldToGrid(transform.position).gridX, level.WorldToGrid(transform.position).gridY);

            var a = transform.position;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject.FindWithTag("MainCamera").transform.position = new Vector3(a.x, a.y, -10);
        }
      //</MOVEMENT> update

      //<CHEATS>
        if (CheatsTurnedOn)
        {
            // Recurrect cheat (Key P)
            if (Input.GetKey("p"))
            {
                Resurrect();
            }

            //Health cheat (Key I): damage
            if (Input.GetKey("i") && isAlive)
            {
                HealthChange(-0.1f);
            }

            //Health cheat (Key O): heal
            if (Input.GetKey("o") && isAlive)
            {
                HealthChange(0.1f);
            }

            //Hunger cheat (Key K): damage
            if (Input.GetKey("k") && isAlive)
            {
                HungerChange(-0.1f);
            }

            //Hunger cheat (Key L): heal
            if (Input.GetKey("l") && isAlive)
            {
                HungerChange(0.1f);
            }

            //Temperature cheat (Key ,): damage
            if (Input.GetKey(",") && isAlive)
            {
                TemperatureChange(-0.1f);
            }

            //Temperature cheat (Key .): heal
            if (Input.GetKey(".") && isAlive)
            {
                TemperatureChange(0.1f);
            }
        }
      //</CHEATS>

    }

  //<HEALTH> void
    //Change health points amount
    public void HealthChange(float amount)
    {
        healthCurrent += amount;
    }

    //Resurrect
    public void Resurrect()
    {
        isAlive = true;
        healthCurrent = healthMax;
        hungerCurrent = hungerMax;
        temperatureCurrent = temperatureMin + temperatureMax;
    }
  //</HEALTH> void

  //<HUNGER> void
    //Change hunger points amount
    public void HungerChange(float amount)
    {
        hungerCurrent += amount;
    }
  //</HUNGER> void

  //<TEMPERATURE> void
    //Change temperature points amount
    public void TemperatureChange(float amount)
    {
        temperatureCurrent += amount;
    }
  //</TEMPERATURE> void
    

}
