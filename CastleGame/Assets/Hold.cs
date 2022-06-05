using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!GameObject.Find("Player").GetComponent<Player>().isPlaying)
        {
            transform.position = new Vector3(mousePosition.x, mousePosition.y, -8.5f);
        }
    }
}
