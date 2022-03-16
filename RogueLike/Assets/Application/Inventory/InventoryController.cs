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

        PlayerController.Instance.weaponController.onBulletShot = SetBullets;
    }

    public void SetHealth(int health) => _currentView.SetHealth(health);
    public void SetMaxHealth(int health) => _currentView.SetMaxHealth(health);

    public void AddWeaponSlot() => _currentView.AddWeaponSlot();

    public void SetWeapons(List<Weapon> weapons)
    {
        _currentWeapon = weapons[0];

        Texture[] weaponImages = new Texture[weapons.Count];
        for (int i = 0; i < weapons.Count; i++)
            weaponImages[i] = weapons[i].weaponImage.texture;

        _currentView.SetWeapons(weaponImages);
    }

    public void SetBullets()
    {

    }

    public void Unload()
    {
        _currentView.Unload();
    }
}
