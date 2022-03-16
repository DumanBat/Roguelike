using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField]
    private WeaponFactory _weaponFactory;
    public WeaponFactory GetWeaponFactory() => _weaponFactory;
    /// TEMP 
    public WeaponType weaponToSpawn = WeaponType.AutoRifle;
    public WeaponType startingWeapon = WeaponType.Sidearm;
    /// 

    public void Init()
    {
        if (PlayerController.Instance.weaponController.weapons.Count == 0)
            PlayerController.Instance.weaponController.AddWeaponToInventory(SpawnWeapon(startingWeapon, Vector3.zero));
    }

    public Weapon SpawnWeapon(WeaponType weaponTypeToSpawn, Vector3 spawnPosition)
    {
        //var weapon = Instantiate(weaponTypeToSpawn, spawnPosition, Quaternion.identity);
        var weapon = _weaponFactory.Get(weaponTypeToSpawn);
        weapon.transform.position = spawnPosition;
        weapon.weaponInGameSprite.gameObject.SetActive(true);
        weapon.onAddedToInventory += PlayerController.Instance.weaponController.AddWeaponToInventory;
        //weapon.Init();
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
