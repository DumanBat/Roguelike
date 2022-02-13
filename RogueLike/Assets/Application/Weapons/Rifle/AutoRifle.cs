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
        bulletsLeft = magazineSize;
        bulletVelocity = 30;
        damage = 10;
        bpm = 7;
        reloadingDuration = 3;
        isAuto = true;

        base.Init();
    }
}
