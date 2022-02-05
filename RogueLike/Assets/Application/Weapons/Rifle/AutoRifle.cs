using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AutoRifle : Weapon
{
    private void OnEnable()
    {
        Init();
    }

    public override void Init()
    {
        magazineSize = 30;
        bulletVelocity = 60;

        base.Init();
    }
}
