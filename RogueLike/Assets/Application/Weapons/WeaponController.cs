using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Modules.Core;

public class WeaponController : MonoBehaviour
{
    private WeaponView _currentView;
    public Weapon currentWeapon;
    public List<Weapon> weapons;

    public Action<int> onBulletAmountChange;
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

    public void Init(List<WeaponConfig> weaponConfigs = null)
    {
        if (weaponConfigs != null)
        {
            foreach (var config in weaponConfigs)
            {
                var weapon = GameManager.Instance.levelManager.GetLevelConfigurator().GetLootManager().GetWeaponFactory().Get(config);
                AddWeaponToInventory(weapon);
            }
        }
    }

    public void Shot(Vector3 aimPos)
    {
        if (currentWeapon.Shot(aimPos))
            onBulletAmountChange?.Invoke(currentWeapon.GetBulletsLeft());
    }

    public void AddWeaponToInventory(Weapon weapon)
    {
        weapons.Insert(0, weapon);
        weapon.AddToInventory(this.transform);
        if (weapons.Count > 1)
            onWeaponChange.Invoke();
        SelectWeapon(0);
        GameManager.Instance.inventoryController.AddWeaponSlot(weapon);
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

    public List<WeaponConfig> GetWeaponConfigs() => weapons.Select(x => x.GetConfig()).ToList();

    public void Unload()
    {
        foreach (var weapon in weapons)
            Destroy(weapon.gameObject);

        weapons.Clear();
    }
}
