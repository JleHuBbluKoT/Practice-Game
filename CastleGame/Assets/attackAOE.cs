using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackAOE : MonoBehaviour
{
    Collider2D coll;
    void Start() {
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 5);
        foreach (var item in colliders) {
            if (item.transform.tag == "Creature") {
                item.SendMessage("ApplyDamage", 20f);
            }
            
        }
        //particles.
        Destroy(gameObject);
    }

}
