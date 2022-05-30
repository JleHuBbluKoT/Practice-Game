using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTest : MonoBehaviour
{
    public float speed = 1f; //Controls velocity multiplier

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
        float yMove = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1
        rb.velocity = new Vector2(xMove, yMove) * speed; // Creates velocity in direction of value equal to keypress (WASD). rb.velocity.y deals with falling + jumping by setting velocity to y. 

    }
}
