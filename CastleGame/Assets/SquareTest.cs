using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTest : MonoBehaviour
{
    public float speed = 1f; //Controls velocity multiplier
    public Level level;

    Rigidbody2D rb;
    
    // Start is called before the first frame update

    public void Set()
    {

    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player

        Vector2Int v = new Vector2Int(level.WorldToGrid(transform.position).gridX, level.WorldToGrid(transform.position).gridY);
        Debug.Log(v + " " + transform.position.x + " " + transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
        float yMove = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1
        
        var h = GameObject.FindWithTag("Player").GetComponent<Health>().currentHealth / GameObject.FindWithTag("Player").GetComponent<Health>().maxHealth; //Health percentage calculation
        rb.velocity = new Vector2(xMove, yMove) * speed * h; // Creates velocity in direction of value equal to keypress (WASD). rb.velocity.y deals with falling + jumping by setting velocity to y. Movement speed depends on healthbat percentage.

        Vector2Int v = new Vector2Int(level.WorldToGrid(transform.position).gridX, level.WorldToGrid(transform.position).gridY);

        var a = GameObject.FindWithTag("Player").transform.position;
        GameObject.FindWithTag("Player").transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        GameObject.FindWithTag("MainCamera").transform.position = new Vector3(a.x, a.y, -10);

    }

}
