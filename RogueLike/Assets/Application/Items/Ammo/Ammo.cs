using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Item
{
    public override void Apply()
    {
        var weapon = PlayerController.Instance.weaponController.GetCurrentWeapon();
        weapon.SetTotalAmmoLeft(weapon.GetMaxTotalAmmo());
    }
}
