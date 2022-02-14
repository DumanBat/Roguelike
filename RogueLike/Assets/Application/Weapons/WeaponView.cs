using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    private int _weaponDirectionIndex = -1;

    public ProgressBar progressBar;

    public void DisableWeaponSprites(Weapon currentWeapon)
    {
        foreach (var weapon in currentWeapon.weaponSprites)
            weapon.SetActive(false);
    }

    public void SetActiveWeaponSprite(Weapon currentWeapon, int directionIndex)
    {
        currentWeapon.weaponSprites[directionIndex].SetActive(true);

        if (_weaponDirectionIndex == directionIndex) return;
        _weaponDirectionIndex = directionIndex;
        DisableWeaponSprites(currentWeapon);
    }

    public void FillReloadProgressBar(float duration)
    {
        progressBar.SetValue(0);
        var timeLft = duration;
        /*while (duration - Time.deltaTime > 0)
        {
            timeLft = duration - Time.deltaTime;
            if (timeLft != 0)
                progressBar.IncrementValue(1 / timeLft);
        }*/
    }
}
