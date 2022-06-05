using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDummy : Interactible
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ApplyDamage(float Damage)
    {
        Destroy(gameObject);
    }
    public override void Interact()
    {

    }
}
