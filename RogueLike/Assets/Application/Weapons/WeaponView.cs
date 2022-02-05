using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    private int _weaponDirectionIndex = -1;

    public void DisableWeaponSprites(Weapon currentWeapon)
    {
        foreach (var weapon in currentWeapon.weaponSprites)
            weapon.SetActive(false);
    }

    public void SetActiveWeaponSprite(Weapon currentWeapon, int directionIndex)
    {
        if (currentWeapon.weaponSprites[directionIndex].activeSelf)
            return;

        currentWeapon.weaponSprites[directionIndex].SetActive(true);

        if (_weaponDirectionIndex == directionIndex) return;
        _weaponDirectionIndex = directionIndex;
        DisableWeaponSprites(currentWeapon);
    }
}
