using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class LootManager : MonoBehaviour
{
    [SerializeField]
    private WeaponFactory _weaponFactory;
    public WeaponFactory GetWeaponFactory() => _weaponFactory;

    [SerializeField]
    private ItemFactory _itemFactory;

    public ItemFactory GetItemFactory() => _itemFactory;
    /// TEMP 
    public WeaponType weaponToSpawn = WeaponType.AutoRifle;
    private WeaponType startingWeapon = WeaponType.Sidearm;
    /// 

    public void Init()
    {
        if (PlayerController.Instance.weaponController.GetWeapons().Count == 0)
            PlayerController.Instance.weaponController.AddWeaponToInventory(SpawnWeapon(startingWeapon, Vector3.zero));
    }

    public Weapon SpawnWeapon(WeaponType weaponTypeToSpawn, Vector3 spawnPosition)
    {
        var weapon = _weaponFactory.Get(weaponTypeToSpawn);
        weapon.transform.position = spawnPosition;
        weapon.weaponInGameSprite.gameObject.SetActive(true);
        weapon.onAddedToInventory += PlayerController.Instance.weaponController.AddWeaponToInventory;
        return weapon;
    }

    public Item SpawnItem(ItemType itemTypeToSpawn, Vector3 spawnPosition)
    {
        var item = _itemFactory.Get(itemTypeToSpawn);
        item.transform.position = spawnPosition;
        item.itemInGameSprite.gameObject.SetActive(true);
        return item;
    }

    public void SpawnLootInRoom(Room room)
    {
        if (room.roomType != RoomTemplates.RoomType.LootRoom)
            return;

        var lootType = Random.Range(0, 2);

        if (lootType == 0)
        {
            var allWeapons = GameManager.Instance.levelManager.GetWeaponTypeHandler().GetAllWeapons();

            var weaponType = allWeapons[Random.Range(0, allWeapons.Count)];
            var weapon = SpawnWeapon(weaponType, room.transform.position);
            weapon.onPickUp += room.OpenDoors;
        }
        else
        {
            var allItems = GameManager.Instance.levelManager.GetItemTypeHandler().GetAllItems();
            var itemType = allItems[Random.Range(0, allItems.Count)];
            var item = SpawnItem(itemType, room.transform.position);
            item.onPickUp += room.OpenDoors;
        }
    }
}
