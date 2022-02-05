using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private InventoryView _currentView;

    private void Awake()
    {
        _currentView = GetComponent<InventoryView>();
    }

    public void SwapWeapons()
    {

    }

    public void SetWeapons(List<Weapon> weapons)
    {
        var length = weapons.Count >= 3
            ? 3
            : weapons.Count;

        Texture[] weaponImages = new Texture[length];
        for (int i = 0; i < length; i++)
            weaponImages[i] = weapons[i].weaponImage.texture;

        _currentView.SetWeapons(weaponImages);
    }
}
