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
        bulletsLeft = magazineSize;
        bulletVelocity = 15;
        damage = 1;
        bpm = 5;
        reloadingDuration = 1.5f;
        isAuto = false;

        base.Init();
    }
}
