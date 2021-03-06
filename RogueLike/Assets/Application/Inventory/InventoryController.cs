using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private InventoryView _currentView;
    private Weapon _currentWeapon;

    private void Awake()
    {
        _currentView = GetComponent<InventoryView>();
    }

    public void Init()
    {
        PlayerController.Instance.onHealthChange = SetHealth;
        PlayerController.Instance.onMaxHealthChange = SetMaxHealth;

        PlayerController.Instance.weaponController.onBulletAmountChange = SetBullets;
    }

    public void SetHealth(int health) => _currentView.SetHealth(health);
    public void SetMaxHealth(int health) => _currentView.SetMaxHealth(health);

    public void AddWeaponSlot(Weapon weapon)
    {
        _currentWeapon = weapon;
        _currentView.AddWeaponSlot(weapon);
        SetBullets();
    }

    public void SetWeapons(List<Weapon> weapons)
    {
        _currentWeapon = weapons[0];
        _currentView.SetWeapons();
        SetBullets();
    }

    public void SetBullets()
    {
        var magazineAmmo = _currentWeapon.GetMagazineAmmoLeft();
        var totalAmmo = $"{_currentWeapon.GetTotalAmmoLeft()}/{_currentWeapon.GetMaxTotalAmmo()}";

        _currentView.SetBullets(magazineAmmo, totalAmmo);
    }

    public void Unload()
    {
        _currentView.Unload();
    }
}
