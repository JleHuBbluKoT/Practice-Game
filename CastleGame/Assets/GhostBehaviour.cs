using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GhostBehaviour : Creature
{
    public List<GridNode> angryNodes = new List<GridNode>();
    float elapsed = 0f;
    float lookCooldown = 0f;
    //float angryCooldown = 0f;
    public float speed = 4.5f;
    public float awareness = 0;
    //bool pathfindChase = false;
    bool longRangeChase = false;
    GridNode myNode = new GridNode(true, Vector2Int.zero, 0,0,"FLOOR");

    bool isCharging = false;
    float charging = 0f;
    float chargingCooldown = 0f;
    GridNode needNode;

    Rigidbody2D rb;

    public float stopTime = 0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player    
        pathNodes = new List<GridNode>();
        //myNode = level.WorldToGrid(this.transform.position);
    }

    void Update()
    {
        var isPlaying = GameObject.Find("Player").GetComponent<Player>().isPlaying;
        var isAlive = GameObject.Find("Player").GetComponent<Player>().isAlive;

        if (isAlive && isPlaying && stopTime <= 0f)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1f);

            //???????? ? ???????? ??????
            if (level.WorldToGrid(this.transform.position) != myNode)
            {
                if (myNode != null)
                {
                    if (myNode.associatedObject != null)
                    {
                        myNode.associatedObject.GetComponent<DoorObject>().Close();
                    }
                    myNode = level.WorldToGrid(this.transform.position);
                }
            }

            if (myNode.associatedObject != null)
            {
                myNode.associatedObject.GetComponent<DoorObject>().GhostINteract();
            }
            // ????? ???????? ????????. ??? ^^^ ????? ???????????? ? ????? ?????? ?? ?????? ????????? ?????

            chargingCooldown = Mathf.Clamp(chargingCooldown - Time.deltaTime, 0, 3);
            elapsed += Time.deltaTime;
            lookCooldown += Time.deltaTime;
            if (elapsed >= 5f && awareness < 100)
            {
                elapsed = 0;
                if (pathNodes.Count == 0)
                {// ???? ??? ????, ?????????? ? ????????? ???????
                    Vector2Int a = new Vector2Int(level.WorldToGrid(this.transform.position).gridX, level.WorldToGrid(this.transform.position).gridY);
                    int randRoom = UnityEngine.Random.Range(0, level.levelRoomList.Count - 1);
                    GridNode bbb = level.levelRoomList[randRoom].QuickFreeSpot();
                    Vector2Int b = new Vector2Int(bbb.gridX, bbb.gridY);
                    pathNodes = AStar(a, b);
                }
            }
            //Awareness - ???????? ?? ?? ????????? ??????? ??????????? ?? ??????. ????? 100 - ???????? ??? ? ????????.
            if (lookCooldown >= 0.1f)
            {
                lookCooldown = 0;
                if (CanSee(playerR))
                {
                    awareness = Mathf.Clamp(awareness + 6 + 5 / Mathf.Clamp(distance(playerR), 1, 10) + awareness / 150, 0, 300);
                }
                else
                {
                    awareness = Mathf.Clamp(awareness - 2, 0, 300);
                }
            }


            if (isCharging && (chargingCooldown == 0))
            {
                charging += Time.deltaTime;
            }
            if (charging > 2f)
            {
                charging = 0;
                chargingCooldown = 1f;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 3);
                foreach (var item in colliders)
                {
                    Debug.Log(item.transform.tag);
                    if (item.transform.tag == "Creature" || item.transform.tag == "Interactible")
                    {
                        item.SendMessage("ApplyDamage", 35f);
                    }

                }

                /*
                GameObject e = Instantiate(AOE);
                e.transform.position = transform.position;*/
            }


            if (awareness >= 100)
            {
                pathNodes.Clear();

                if (distance(playerR) < 3.5f)
                {
                    //pathfindChase = false;
                    isCharging = true;
                }
                //else {  pathfindChase = true; }

                //????? ??? ????? ??? ?? ?????? ?? ???-?? ????? ? ?????????

                //else 
                {
                    if (!longRangeChase)
                    {
                        pathNodes.Clear();
                        Vector2Int a = new Vector2Int(level.WorldToGrid(this.transform.position).gridX, level.WorldToGrid(this.transform.position).gridY);
                        Vector2Int b = new Vector2Int(level.WorldToGrid(playerR.transform.position).gridX, level.WorldToGrid(playerR.transform.position).gridY);
                        angryNodes = AStar(a, b, false);
                        if (angryNodes.Count > 2)
                        {
                            needNode = angryNodes[1];
                        }
                        longRangeChase = true;
                    }
                    else
                    {
                        if (needNode != null)
                        {
                            Vector2 nextNodePost = level.GridToWorld(new Vector2Int(needNode.gridX, needNode.gridY));
                            rb.velocity = angleBetweenPoints(this.transform.position, new Vector2(nextNodePost.x, nextNodePost.y)) * speed * Mathf.Clamp(awareness / 200, 1, 1.50f);
                            Vector2 targetNode = level.GridToWorld(new Vector2Int(needNode.gridX, needNode.gridY));
                            if ((Math.Abs(targetNode.x - transform.position.x) < 0.2f) && (Math.Abs(targetNode.y - transform.position.y) < 0.20f))
                            {
                                needNode = null;
                                longRangeChase = false;
                            }

                        }
                        else { longRangeChase = false; }
                    }



                }

            }
            else
            {
                angryNodes.Clear();
                needNode = null;
                if (pathNodes.Count > 0)
                {//????????? ?? ???? ?? ????
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
        else
        {
            if (isAlive && isPlaying)
            {
                if (stopTime > 0f)
                {
                    stopTime -= 1f;
                }
            }
            rb.velocity = new Vector3(0, 0, 0);
        }

        
    }
    public override void ApplyDamage(float Damage)
    {

    }
    public void Restart()
    {
         pathNodes.Clear();
         angryNodes.Clear();
         elapsed = 0f;
         lookCooldown = 0f;

         speed = 4.5f;
         awareness = 0;

         longRangeChase = false;
         myNode = new GridNode(true, Vector2Int.zero, 0, 0, "FLOOR");

         isCharging = false;
         charging = 0f;
         chargingCooldown = 0f;
         needNode = null;
    }

    public bool CanSee(GameObject target)
    {
        Vector2 startP = this.transform.position;
        Vector2 startEP = target.transform.position;
        Vector3 diff = new Vector3(startEP.x - startP.x, startEP.y - startP.y, 0);
        //float distance = (float)(Mathf.Abs((float)(Mathf.Min(diff.x, diff.y) * 1.5)) + Mathf.Abs(diff.x - diff.y));

        RaycastHit2D hit = Physics2D.Raycast(startP, diff, Mathf.Infinity, 1 << 3);
        if (hit.transform.tag == playerR.transform.tag)
        {
            return true;
        }
        return false;
    }

    public void StopGhost(float addTime)
    {
        stopTime += addTime;
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