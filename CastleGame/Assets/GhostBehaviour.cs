using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GhostBehaviour : Creature
{
    public GameObject AOE;
    public List<GridNode> angryNodes = new List<GridNode>();
    float elapsed = 0f;
    float lookCooldown = 0f;
    //float angryCooldown = 0f;
    public float speed = 1f;
    public float awareness = 0;
    //bool pathfindChase = false;
    bool longRangeChase = false;
    GridNode myNode = new GridNode(true, Vector2Int.zero, 0,0,"FLOOR");

    bool isCharging = false;
    float charging = 0f;
    float chargingCooldown = 0f;
    GridNode needNode;

    Rigidbody2D rb;
    void Start() {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player    
        pathNodes = new List<GridNode>();
        //myNode = level.WorldToGrid(this.transform.position);
    }

    async void Update()
    {
        //ќткрытие и закрытие дверей
        if (level.WorldToGrid(this.transform.position) != myNode) { 
            if (myNode != null) {
                if (myNode.associatedObject != null) {
                    myNode.associatedObject.GetComponent<DoorObject>().Close();
                }
                myNode = level.WorldToGrid(this.transform.position);
            }
        }

        if (myNode.associatedObject != null) {
            myNode.associatedObject.GetComponent<DoorObject>().GhostINteract();
        }
        //  онец открыти€ закрыти€. Ёто ^^^ можно закомментить и тогда монстр не сможет открывать двери

        chargingCooldown = Mathf.Clamp(chargingCooldown - Time.deltaTime, 0, 3);
        elapsed += Time.deltaTime;
        lookCooldown += Time.deltaTime;
        if (elapsed >= 5f && awareness < 100) {
            elapsed = 0;
            if (pathNodes.Count == 0) {// если нет цели, отправл€ет в случайную комнату
                Vector2Int a = new Vector2Int(level.WorldToGrid(this.transform.position).gridX, level.WorldToGrid(this.transform.position).gridY);
                int randRoom = UnityEngine.Random.Range(0, level.levelRoomList.Count - 1);
                Vector2Int b = level.levelRoomList[randRoom].center();
                pathNodes = AStar(a, b);
            }
        }
        //Awareness - отвечает за то насколько бабайка осведомлена об игроке.  огда 100 - замечает его и нападает.
        if (lookCooldown >= 0.1f) {
            lookCooldown = 0;
            if (CanSee(playerR)) {
                awareness = Mathf.Clamp(awareness + 10 + 10 / Mathf.Clamp(distance(playerR), 1, 10) + awareness / 100, 0, 300);
            }
            else {
                awareness = Mathf.Clamp(awareness - 2, 0, 300);
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 2);
        foreach (var item in colliders) {
            if (item.transform.tag == "Interactible") {
                item.SendMessage("ApplyDamage", 20f);
            }

        }

        



        if (isCharging && (chargingCooldown == 0)) {
            charging += Time.deltaTime;
        }
        if (charging > 2f)
        {
            charging = 0;
            chargingCooldown = 3f;
            GameObject e = Instantiate(AOE);
            e.transform.position = transform.position;
        }


        if (awareness >= 100) {
            pathNodes.Clear();
            
            if (distance(playerR) < 3.5f) {
                //pathfindChase = false;
                isCharging = true;
            } 
            //else {  pathfindChase = true; }
            /*
            здесь был умный код но теперь он где-то внизу в комментах
            */
            //else 
            {
                if (!longRangeChase) {
                    pathNodes.Clear();
                    Vector2Int a = new Vector2Int(level.WorldToGrid(this.transform.position).gridX, level.WorldToGrid(this.transform.position).gridY);
                    Vector2Int b = new Vector2Int(level.WorldToGrid(playerR.transform.position).gridX, level.WorldToGrid(playerR.transform.position).gridY);
                    angryNodes = AStar(a, b, false);
                    if (angryNodes.Count > 2) {
                        needNode = angryNodes[1];
                    }
                    longRangeChase = true;
                } 
                else {
                    if (needNode != null) {
                        Vector2 nextNodePost = level.GridToWorld(new Vector2Int(needNode.gridX, needNode.gridY));
                        rb.velocity = angleBetweenPoints(this.transform.position, new Vector2(nextNodePost.x, nextNodePost.y)) * speed * Mathf.Clamp(awareness/200, 1, 1.50f);
                        Vector2 targetNode = level.GridToWorld(new Vector2Int(needNode.gridX, needNode.gridY));
                        if ((Math.Abs(targetNode.x - transform.position.x) < 0.2f) && (Math.Abs(targetNode.y - transform.position.y) < 0.20f)) {
                            needNode = null;
                            longRangeChase = false;
                        }

                    }
                    else { longRangeChase = false; }
                }
                

                
            }
            
        }
        else {
            angryNodes.Clear();
            needNode = null;
            if (pathNodes.Count > 0)
            {//Ќавигаци€ от ноды до ноды
                Vector2 nextNodePost = level.GridToWorld(new Vector2Int(pathNodes[0].gridX, pathNodes[0].gridY));
                rb.velocity = angleBetweenPoints(this.transform.position, nextNodePost) * speed * (100 - Mathf.Clamp(awareness, 0, 100)) / 100;
                Vector2 targetNode = level.GridToWorld(new Vector2Int(pathNodes[0].gridX, pathNodes[0].gridY));
                if ((Math.Abs(targetNode.x - transform.position.x) < 0.2f) && (Math.Abs(targetNode.y - transform.position.y) < 0.20f))
                {
                    pathNodes.RemoveAt(0);
                }
            }
            else { rb.velocity = Vector2.zero; }
        }



    }

    public bool CanSee(GameObject target) {
        Vector2 startP = this.transform.position;
        Vector2 startEP = target.transform.position; 
        Vector3 diff = new Vector3(startEP.x - startP.x, startEP.y - startP.y, 0);
        //float distance = (float)(Mathf.Abs((float)(Mathf.Min(diff.x, diff.y) * 1.5)) + Mathf.Abs(diff.x - diff.y));

        RaycastHit2D hit = Physics2D.Raycast(startP, diff);
        if (hit.transform.tag == playerR.transform.tag) {
            return true;
        }
        return false;
    }

}
/*
        if (Input.GetKeyDown("r"))
        {
            Vector2Int a = new Vector2Int(level.WorldToGrid(this.transform.position).gridX, level.WorldToGrid(this.transform.position).gridY);
            Vector2Int b = new Vector2Int(level.WorldToGrid(playerR.transform.position).gridX, level.WorldToGrid(playerR.transform.position).gridY);
            pathNodes = AStar(a, b);
        } */


/*
            if (!pathfindChase) {
                Vector2 nextNodePost = playerR.transform.position;
                rb.velocity = angleBetweenPoints(this.transform.position, nextNodePost) * speed * Mathf.Clamp(awareness / 150, 1, 1.60f);
                pathNodes.Clear();
            }*/


/*
else
{
    if (angryNodes.Count == 0)
    {
        pathNodes.Clear();
        Vector2Int a = new Vector2Int(level.WorldToGrid(this.transform.position).gridX, level.WorldToGrid(this.transform.position).gridY);
        Vector2Int b = new Vector2Int(level.WorldToGrid(playerR.transform.position).gridX, level.WorldToGrid(playerR.transform.position).gridY);
        angryNodes = AStar(a, b);
    }


    if (angryNodes.Count > 0)
    {
        Vector2 nextNodePost = level.GridToWorld(new Vector2Int(angryNodes[0].gridX, angryNodes[0].gridY));
        rb.velocity = angleBetweenPoints(this.transform.position, nextNodePost) * speed * 1.2f;
        Vector2 targetNode = level.GridToWorld(new Vector2Int(angryNodes[0].gridX, angryNodes[0].gridY));
        if ((Math.Abs(targetNode.x - transform.position.x) < 0.2f) && (Math.Abs(targetNode.y - transform.position.y) < 0.20f))
        {
            angryNodes.RemoveAt(0);
        }
    }
}*/