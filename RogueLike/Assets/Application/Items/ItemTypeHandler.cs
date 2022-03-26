using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTypeHandler : MonoBehaviour
{
    private List<ItemType> _itemsTierThree = new List<ItemType>()
    {
        ItemType.Heart,
        ItemType.Ammo
    };
    public List<ItemType> GetItemsTierThree() => _itemsTierThree;

    public List<ItemType> GetAllItems()
    {
        var allItems = new List<ItemType>();
        allItems.AddRange(_itemsTierThree);

        return allItems;
    }
}
