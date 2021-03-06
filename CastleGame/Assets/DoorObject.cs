using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : Interactible
{
    public GameObject door1;
    public GameObject door2;

    public bool isOpen;
    public bool horizontal;
    public float ghostCounter;
    
    void Start() {
        if (horizontal) {
            this.transform.rotation =  Quaternion.Euler(0f,0f,90f);
        }
    }


    public override void Interact()
    {
        if (isOpen && ghostCounter ==0) {
            this.Close();
            Wait(1f);
        }
        else {
            this.Open();
            Wait(1f);
        }
    }

    public void GhostINteract() {
        ghostCounter = 3f;
        Open();
    }

    void Update() 
    {
        ghostCounter = Mathf.Clamp( ghostCounter - Time.deltaTime, 0, 3 );
    }
    public void Open() {
        isOpen = true;
        if (horizontal) {
            door2.transform.position = new Vector3(this.transform.position.x - 1.5f, this.transform.position.y, -9);
            door1.transform.position = new Vector3(this.transform.position.x + 1.5f, this.transform.position.y, -9);
        } else {
            door1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1.5f, -9);
            door2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, -9);
        }    
    }
    public void Close() {
        isOpen = false;
        if (horizontal) {
            door2.transform.position = new Vector3(this.transform.position.x - 0.5f, this.transform.position.y, -9);
            door1.transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y, -9);
        } else {
            door1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, -9);
            door2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, -9);
        }
    }

    IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
    }

    public override void ApplyDamage(float Damage) {
    }
}
