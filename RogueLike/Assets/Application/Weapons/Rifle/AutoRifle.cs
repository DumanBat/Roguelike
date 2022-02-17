using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AutoRifle : Weapon
{
    public override void Init()
    {
        magazineSize = 30;
        bulletsLeft = magazineSize;
        bulletVelocity = 30;
        damage = 10;
        bpm = 7;
        reloadingDuration = 3f;
        isAuto = true;
        base.Init();
    }
}
