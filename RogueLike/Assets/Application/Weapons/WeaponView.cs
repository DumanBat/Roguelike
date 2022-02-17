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

    public IEnumerator FillReloadProgressBar(float duration)
    {
        progressBar.gameObject.SetActive(true);
        progressBar.SetValue(0);
        float time = 0.0f;
        while (time < duration)
        {
            progressBar.IncrementValue(time/duration);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        progressBar.gameObject.SetActive(false);
    }
}
