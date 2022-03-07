using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    /// TEMP 
    public Weapon weaponToSpawn;
    public Weapon startingWeapon;
    /// 

    public void Init()
    {
        if (PlayerController.Instance.weaponController.weapons.Count == 0)
            PlayerController.Instance.weaponController.AddWeaponToInventory(SpawnWeapon(startingWeapon, Vector3.zero));
    }

    public Weapon SpawnWeapon(Weapon weaponToSpawn, Vector3 spawnPosition)
    {
        var weapon = Instantiate(weaponToSpawn, spawnPosition, Quaternion.identity);
        weapon.weaponInGameSprite.gameObject.SetActive(true);
        weapon.onAddedToInventory += PlayerController.Instance.weaponController.AddWeaponToInventory;
        weapon.Init();
        return weapon;
    }

    public void SpawnLootInRoom(Room room)
    {
        if (room.roomType != RoomTemplates.RoomType.LootRoom)
            return;

        var weapon = SpawnWeapon(weaponToSpawn, room.transform.position);
        weapon.onWeaponPickUp += room.OpenDoors;
    }
}
