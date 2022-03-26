using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Modules.Core;

public class WeaponController : MonoBehaviour
{
    private WeaponView _currentView;
    [SerializeField]
    private Weapon _currentWeapon;
    public Weapon GetCurrentWeapon() => _currentWeapon;
    private List<Weapon> _weapons = new List<Weapon>();
    public List<Weapon> GetWeapons() => _weapons;

    public Action onBulletAmountChange;
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

    public bool Shot(Vector3 aimPos) => _currentWeapon.Shot(aimPos);

    public void AddWeaponToInventory(Weapon weapon, bool isEnemyWeapon = false)
    {
        _weapons.Insert(0, weapon);
        weapon.AddToInventory(this.transform);
        if (_weapons.Count > 1)
            onWeaponChange.Invoke();
        SelectWeapon(0);

        if (!isEnemyWeapon)
            GameManager.Instance.inventoryController.AddWeaponSlot(weapon);
    }

    private void SelectWeapon(int index)
    {
        _currentWeapon = _weapons[index];
    }

    public void SwapWeapon()
    {
        onWeaponChange.Invoke();
        _weapons.Add(_weapons[0]);
        _weapons.RemoveAt(0);
        SelectWeapon(0);
        GameManager.Instance.inventoryController.SetWeapons(_weapons);
    }

    public void SetActiveWeaponDirection(int directionIndex)
    {
        if (_currentWeapon.weaponSprites[directionIndex].activeSelf)
            return;

        _currentWeapon.SetFirepoint(_currentWeapon.weaponSprites[directionIndex].transform.GetChild(0));

        _currentView.SetActiveWeaponSprite(_currentWeapon, directionIndex);
    }

    public void DisableWeaponSprites()
    {
        _currentView.DisableWeaponSprites(_currentWeapon);
    }

    public void Reload()
    {
        _reloadingRoutine = StartCoroutine(_currentWeapon.Reload());
    }

    public void FillReloadProgressBar(float duration)
    {
        _reloadingViewRoutine = StartCoroutine(_currentView.FillReloadProgressBar(duration));
    }

    public void StopReloading()
    {
        if (!_currentWeapon.IsReloading()) 
            return;

        StopCoroutine(_reloadingRoutine);
        StopCoroutine(_reloadingViewRoutine);
        _currentWeapon.StopReloading();
        _currentView.ResetReloadProgressBar();
    }

    public List<WeaponConfig> GetWeaponConfigs() => _weapons.Select(x => x.GetConfig()).ToList();

    public void Unload()
    {
        foreach (var weapon in _weapons)
        {
            weapon.CleanBulletPool();
            Destroy(weapon.gameObject);
        }

        _weapons.Clear();
    }
}
