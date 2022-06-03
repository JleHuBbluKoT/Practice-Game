using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isPlaying = true; //Pause
    public bool cheatsTurnedOn = false; //Cheats

    public bool isAlive = true; //Is player alive check
    public static Collider[] hitColliders;

    //<HEALTH> data
    [System.Serializable]
    public class Health
    {
        public float max = 100f; //Max player health points
        public float current = 100f; //Current player health points

        //Player health points percentage
        public static float percent = 1f;
    }

    public Health health = new Health();

    //<HUNGER> data
    [System.Serializable]
    public class Hunger
    {
        public float max = 100f; //Max player hunger points
        public float current = 100f; //Current player hunger points
        public float recoverMinPercent = 0.5f; //Minimum of hunger bar percent to start regenerating health
        public float recoverValue = 0.0025f; //Health recovery, when hunger percent is larger or equal to hungerRecoverMinPercent
        public float damage = 0.005f; //Damage by being hungry
        public float naturalDecrease = 0.0005f; //Natural hunger decrease by time

        //Player hunger points percentage
        public static float percent = 1f;
    }
    public Hunger hunger = new Hunger();

    //<TEMPERATURE> data
    [System.Serializable]
    public class Temperature
    {
        public float min = -50f; //Min available player temperature. Less then this value kills the player.
        public float max = 50f; //Max available player temperature. More then this value kills the player.
        public float current = 0f; //Current player temperature
        public float damage = 0.02f; //High or low temperature damage

        //Player temerature points percentage
        public static float percent = 0f;
    }
    public Temperature temperature = new Temperature();


    //<MOVEMENT> data
    public float speed = 1f; //Controls velocity multiplier
    public Level level;
    public float playerReach = 10f;

    Rigidbody2D rb;

    // PLAYER DATA
    GridNode lookAtNode;

    // <INVENTORY> data
    [System.Serializable]
    public class Slot
    {
        public int ID; //Item ID
        public int amount;
    }
    public float itemReach = 1f; //How far player can reach items
    public int inventoryLimit = 20; //Maximum item slots amount
    public List<Slot> inventory = new List<Slot>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player    
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); //No rotation

        if (isPlaying)
        {
            //<PLAYING>

            //Applying death due 0 or less healthpoints
            if (isAlive)
            {
                if (health.current <= 0f)
                {
                    isAlive = false;
                    Debug.Log("YOU DIED");
                }
            }

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
                            //level.objectLib.GetComponent<GameObjectLib>().remove(new Vector2Int(lookAtNode.gridX, lookAtNode.gridY), level);
                            lookAtNode.associatedObject.GetComponent<Interactible>().Interact();
                        }
                    }
                }
            }

            //Bar update if player is not dead
            if (isAlive)
            {
                //Bar positions
                var barPosition = GameObject.Find("Bar").transform.position;
                var barScale = GameObject.Find("Bar").transform.localScale;

                //<HEALTH> update
                //Health maximum check
                if (health.current > health.max)
                {
                    health.current = health.max;
                }

                //Health minimum check
                if (health.current < 0f)
                {
                    health.current = 0f;
                }

                //Percantage of current health compared to the maximum
                Health.percent = health.current / health.max;

                //Resizing part of the health bar
                GameObject.Find("Health").transform.position = new Vector3(barPosition.x - (1 - Health.percent) / 2 * barScale.x,
                    barPosition.y + barScale.y / 3, barPosition.z - 0.1f);
                GameObject.Find("Health").transform.localScale = new Vector3(Health.percent, 0.33333333f, 1f);

                //<HUNGER> update
                //Natural hunger decrease by time
                hunger.current -= hunger.naturalDecrease;

                //Hunger maximum check
                if (hunger.current > hunger.max)
                {
                    hunger.current = hunger.max;
                }

                //Hunger minimum check
                if (hunger.current < 0f)
                {
                    hunger.current = 0f;
                }

                //Percantage of current hunger compared to the maximum
                Hunger.percent = hunger.current / hunger.max;

                //Resizing part of the hunger bar
                GameObject.Find("Hunger").transform.position = new Vector3(barPosition.x - (1 - Hunger.percent) / 2 * barScale.x,
                    barPosition.y, barPosition.z - 0.1f);
                GameObject.Find("Hunger").transform.localScale = new Vector3(Hunger.percent, 0.33333333f, 1f);


                //Max hunger bar recovers health
                if (Hunger.percent >= hunger.recoverMinPercent)
                {
                    health.current += hunger.recoverValue;
                }

                //Hunger damage
                if (hunger.current <= 0)
                {
                    health.current -= hunger.damage;
                }

                //<TEMPERATURE> update
                //Temperature minimum check
                if (temperature.current < temperature.min)
                {
                    temperature.current = temperature.min;
                }

                //Temperature maximum check
                if (temperature.current > temperature.max)
                {
                    temperature.current = temperature.max;
                }

                //Percantage of current temperature compared to the minimum and the maximum
                Temperature.percent = (temperature.current - temperature.min) / (temperature.max - temperature.min);

                //Resizing part of the temperature bar
                GameObject.Find("Temperature").transform.position = new Vector3(barPosition.x - (1 - Temperature.percent) / 2 * barScale.x,
                    barPosition.y - barScale.y / 3, barPosition.z - 0.1f);
                GameObject.Find("Temperature").transform.localScale = new Vector3(Temperature.percent, 0.33333333f, 1f);

                //Changing color of the temperature bar
                GameObject.Find("Temperature").GetComponent<SpriteRenderer>().color = new Color(Temperature.percent, 0, 1 - Temperature.percent, 1);


                //Temperature damage
                if (temperature.current <= temperature.min)
                {
                    health.current -= temperature.damage;
                }
                if (temperature.current >= temperature.max)
                {
                    health.current -= temperature.damage;
                }
            }


            //<MOVEMENT> update
            if (isAlive)
            {
                float xMove = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
                float yMove = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1

                rb.velocity = new Vector2(xMove, yMove) * speed * Health.percent; // Creates velocity in direction of value equal to keypress (WASD). rb.velocity.y deals with falling + jumping by setting velocity to y. Movement speed depends on healthbat percentage.

                //Diagonal speed adjustment
                if (xMove != 0 && yMove != 0)
                {
                    rb.velocity /= 1.41421356237f;
                }

                Vector2Int v = new Vector2Int(level.WorldToGrid(transform.position).gridX, level.WorldToGrid(transform.position).gridY);

                var a = transform.position;
                GameObject.FindWithTag("MainCamera").transform.position = new Vector3(a.x, a.y, -10);
            }

            //<CHEATS>
            if (cheatsTurnedOn)
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
                //Generating random item by right clicking mouse on the screen (as input: player x, player y, item id (-1 - random))
                if (Input.GetMouseButtonUp(1))
                {
                    GameObject.FindWithTag("ItemSpawn").GetComponent<Items>().CreateItem(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);
                }
            }
            //obj.GetComponent<ItemData>().ID
            //Destroy(obj)
            //<INVENTORY> update
            var itemAdded = false;
            if (inventoryLimit > inventory.Count)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) <= itemReach)
                    {
                        itemAdded = false;
                        //Debug.Log(obj.name);
                        for (int i1 = 0; i1 < inventory.Count; i1++)
                        {
                            if (inventory[i1].ID == obj.GetComponent<ItemData>().ID)
                            {
                                inventory[i1].amount += 1;
                                Destroy(GameObject.Find(obj.name));
                                itemAdded = true;
                            }
                        }
                        if (!itemAdded)
                        {
                            inventory.Add(new Slot { ID = obj.GetComponent<ItemData>().ID, amount = 1 });
                            Destroy(GameObject.Find(obj.name));
                        }
                    }
                }


            }

            for (int i1 = 0; i1 < inventory.Count; i1++)
            {
                if (inventory[i1].amount <= 0)
                {
                    inventory.RemoveAt(i1);
                }
            }


            //</PLAYING>
        }
    }

    //<HEALTH> void
    //Change health points amount
    public void HealthChange(float amount)
    {
        health.current += amount;
    }

    //Resurrect
    public void Resurrect()
    {
        isAlive = true;
        health.current = health.max;
        hunger.current = hunger.max;
        temperature.current = temperature.min + temperature.max;
    }

    //<HUNGER> void
    //Change hunger points amount
    public void HungerChange(float amount)
    {
        hunger.current += amount;
    }

    //<TEMPERATURE> void
    //Change temperature points amount
    public void TemperatureChange(float amount)
    {
        temperature.current += amount;
    }


}
