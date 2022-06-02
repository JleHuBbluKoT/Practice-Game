using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : Interactible
{
    public GameObject door1;
    public GameObject door2;

    public bool isOpen;
    public bool horizontal;
    
    void Start() {
        if (horizontal) {
            this.transform.rotation =  Quaternion.Euler(0f,0f,90f);
        }
    }

    public override void Interact()
    {
        if (isOpen) {
            this.Close();
            Wait(1f);
        }
        else {
            this.Open();
            Wait(1f);
        }
    }

    void Update() {
    }
    public void Open() {
        isOpen = true;
        if (horizontal) {
            door2.transform.position = new Vector3(this.transform.position.x - 1.5f, this.transform.position.y, 0);
            door1.transform.position = new Vector3(this.transform.position.x + 1.5f, this.transform.position.y, 0);
        } else {
            door1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1.5f, 0);
            door2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, 0);
        }    
    }
    public void Close() {
        isOpen = false;
        if (horizontal) {
            door2.transform.position = new Vector3(this.transform.position.x - 0.5f, this.transform.position.y, 0);
            door1.transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y, 0);
        } else {
            door1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0);
            door2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, 0);
        }
    }

    IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
