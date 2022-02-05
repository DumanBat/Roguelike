using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public GameObject root;
    public VerticalLayoutGroup weaponsLayoutGroup;
    public List<WeaponSlot> weaponSlots;

    public void InitWeapons()
    {

    }

    public void SetWeapons(Texture[] weaponTextures)
    {
        for (int i = 0; i < weaponTextures.Length; i++)
        {
            weaponSlots[i].imagePlaceholder.texture = weaponTextures[i];
            weaponSlots[i].imagePlaceholder.gameObject.SetActive(true);
        }
    }
}
