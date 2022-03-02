using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class WeaponController : MonoBehaviour
{
    private WeaponView _currentView;
    public Weapon currentWeapon;
    public List<Weapon> weapons;

    public Action<float> onReload;
    public Action onWeaponChange;
    private Coroutine _reloadingRoutine;
    private Coroutine _reloadingViewRoutine;
    private void Awake()
    {
        _currentView = GetComponent<WeaponView>();
        onReload += FillReloadProgressBar;
        onWeaponChange += StopReloading;
        onWeaponChange += DisableWeaponSprites;
    }

    private void Start()
    {
        AddWeapons();
    }

    private void AddWeapons()
    {
        weapons.Clear();

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            weapons.Add(transform.GetChild(i).GetComponent<Weapon>());
            weapons[i].weaponCollider.isTrigger = false;
        }
    }

    public void AddWeaponToInventory(Weapon weapon)
    {
        weapons.Insert(0, weapon);
        weapon.AddToInventory(this.transform);
        if (weapons.Count > 1)
            onWeaponChange.Invoke();
        SelectWeapon(0);
        GameManager.Instance.inventoryController.SetWeapons(weapons);
    }

    private void SelectWeapon(int index)
    {
        currentWeapon = weapons[index];
    }

    public void SwapWeapon()
    {
        onWeaponChange.Invoke();
        weapons.Add(weapons[0]);
        weapons.RemoveAt(0);
        SelectWeapon(0);
        GameManager.Instance.inventoryController.SetWeapons(weapons);
    }

    public void SetActiveWeaponDirection(int directionIndex)
    {
        if (currentWeapon.weaponSprites[directionIndex].activeSelf)
            return;

        currentWeapon.SetFirepoint(currentWeapon.weaponSprites[directionIndex].transform.GetChild(0));

        _currentView.SetActiveWeaponSprite(currentWeapon, directionIndex);
    }

    public void DisableWeaponSprites()
    {
        _currentView.DisableWeaponSprites(currentWeapon);
    }

    public void Reload()
    {
        _reloadingRoutine = StartCoroutine(currentWeapon.Reload());
    }

    public void FillReloadProgressBar(float duration)
    {
        _reloadingViewRoutine = StartCoroutine(_currentView.FillReloadProgressBar(duration));
    }

    public void StopReloading()
    {
        if (!currentWeapon.IsReloading()) 
            return;

        StopCoroutine(_reloadingRoutine);
        StopCoroutine(_reloadingViewRoutine);
        currentWeapon.StopReloading();
        _currentView.ResetReloadProgressBar();
    }
}
