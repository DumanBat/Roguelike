using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    /// TEMP 
    public Weapon weaponToSpawn;
    public Weapon startingWeapon;
    /// 

    private void Start()
    {
        PlayerController.Instance.weaponController.AddWeaponToInventory(SpawnWeapon(startingWeapon, Vector3.zero));
    }

    public Weapon SpawnWeapon(Weapon weaponToSpawn, Vector3 spawnPosition)
    {
        var weapon = Instantiate(weaponToSpawn, spawnPosition, Quaternion.identity);
        weapon.weaponInGameSprite.gameObject.SetActive(true);
        weapon.onWeaponPickUp += PlayerController.Instance.weaponController.AddWeaponToInventory;
        weapon.Init();
        return weapon;
    }
}
