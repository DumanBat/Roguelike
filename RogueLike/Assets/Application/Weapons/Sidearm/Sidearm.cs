using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Sidearm : Weapon
{
    private void OnEnable()
    {
        Init();
    }

    public override void Init()
    {
        magazineSize = 12;
        bulletVelocity = 15;

        base.Init();
    }
}
