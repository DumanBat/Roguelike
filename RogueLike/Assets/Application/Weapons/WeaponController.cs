using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class WeaponController : MonoBehaviour
{
    /// TEMP 
    public Weapon weaponToSpawn;
    /// 
    private WeaponView _currentView;
    public Weapon currentWeapon;
    public List<Weapon> weapons;

    private void Awake()
    {
        _currentView = GetComponent<WeaponView>();
    }

    private void Start()
    {
        AddWeapons();
        currentWeapon = weapons[0];

        SpawnWeapon(weaponToSpawn, new Vector3(10f, 10f, 0f));
        SpawnWeapon(weaponToSpawn, new Vector3(-10f, 10f, 0f));
    }

    private void AddWeapons()
    {
        weapons.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons.Add(transform.GetChild(i).GetComponent<Weapon>());
            weapons[i].weaponCollider.isTrigger = false;
        }
    }

    private void AddWeaponToInventory(Weapon weapon)
    {
        weapons.Insert(0, weapon);
        Destroy(weapon.weaponCollider);
        weapon.transform.SetParent(this.transform);
        weapon.transform.localPosition = Vector3.zero;
        SelectWeapon(0);
        GameManager.Instance.inventoryController.SetWeapons(weapons);
    }

    private void SelectWeapon(int index)
    {
        currentWeapon = weapons[index];
    }

    public void SwapWeapon()
    {
        weapons.Add(weapons[0]);
        weapons.RemoveAt(0);
        SelectWeapon(0);
        GameManager.Instance.inventoryController.SetWeapons(weapons);
    }

    public void SetActiveWeaponSprite(int directionIndex)
    {
        _currentView.SetActiveWeaponSprite(currentWeapon, directionIndex);
    }

    public void DisableWeaponSprites()
    {
        _currentView.DisableWeaponSprites(currentWeapon);
    }

    public void SpawnWeapon(Weapon weaponToSpawn, Vector3 spawnPosition)
    {
        var weapon = Instantiate(weaponToSpawn, spawnPosition, Quaternion.identity);
        weapon.weaponSprites[4].SetActive(true);
        weapon.onWeaponPickUp += AddWeaponToInventory;
        weapon.weaponCollider.isTrigger = true;
    }
}
