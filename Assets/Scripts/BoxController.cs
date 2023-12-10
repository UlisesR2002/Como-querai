using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : Entity
{
    public override void OnDead()
    {
        Destroy(gameObject);
    }

    public override void OnStart()
    {
    }

    public override void OnUpdate()
    {
    }
}
