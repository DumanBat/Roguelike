using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryView : MonoBehaviour
{
    public GameObject root;

    [Header("Weapons")]
    public Transform weaponsRoot;
    public WeaponSlot weaponSlotPrefab;

    public Transform ammoRoot;
    public AmmoSlot ammoSlotPrefab;
    public TextMeshProUGUI totalAmmoDisplay;

    private List<(WeaponSlot, AmmoSlot)> _weaponSlots = new List<(WeaponSlot, AmmoSlot)>();

    [Header("Health")]
    public Transform emptyHeartsRoot;
    public GameObject emptyHeartPrefab;
    private List<GameObject> _emptyHearts = new List<GameObject>();

    public Transform heartsRoot;
    public GameObject heartPrefab;
    private List<GameObject> _hearts = new List<GameObject>();

    public void InitWeapons()
    {

    }

    public void AddWeaponSlot(Weapon weapon)
    {
        var weaponSlot = Instantiate(weaponSlotPrefab, weaponsRoot);
        weaponSlot.imagePlaceholder.texture = weapon.weaponImage.texture;
        weaponSlot.transform.SetAsLastSibling();

        var ammoSlot = Instantiate(ammoSlotPrefab, ammoRoot);
        var bulletSprite = weapon.bulletPrefab.GetComponent<SpriteRenderer>().sprite;
        ammoSlot.SetAmmoSlot(bulletSprite, weapon.GetMagazineSize());

        _weaponSlots.Insert(0, (weaponSlot, ammoSlot));

        foreach (var slot in _weaponSlots)
        {
            slot.Item1.gameObject.SetActive(false);
            slot.Item2.gameObject.SetActive(false);
        }

        _weaponSlots[0].Item1.gameObject.SetActive(true);
        _weaponSlots[0].Item2.gameObject.SetActive(true);
    }

    public void SetWeapons()
    {
        _weaponSlots[0].Item1.gameObject.SetActive(false);
        _weaponSlots[0].Item1.transform.SetAsFirstSibling();
        _weaponSlots[0].Item2.gameObject.SetActive(false);

        _weaponSlots.Add(_weaponSlots[0]);
        _weaponSlots.RemoveAt(0);

        _weaponSlots[0].Item1.gameObject.SetActive(true);
        _weaponSlots[0].Item1.transform.SetAsLastSibling();
        _weaponSlots[0].Item2.gameObject.SetActive(true);
    }

    public void SetBullets(int val, string totalAmmoLeft)
    {
        _weaponSlots[0].Item2.SetBullets(val);
        totalAmmoDisplay.text = totalAmmoLeft;
    }

    public void SetHealth(int health)
    {
        foreach (var heart in _hearts)
            heart.SetActive(false);

        for (int i = 0; i < health; i++)
        {
            _hearts[i].SetActive(true);
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        if (maxHealth % 2 != 0)
            Debug.LogError("Not correct health value! Value must be even");

        var requiredSize = maxHealth / 2;
        var currentSize = emptyHeartsRoot.childCount;

        if (requiredSize > currentSize)
        {
            for (var i = 0; i < requiredSize - currentSize; i++)
            {
                var emptyHeart = Instantiate(emptyHeartPrefab, emptyHeartsRoot);
                _emptyHearts.Add(emptyHeart);
                var heart = Instantiate(heartPrefab, heartsRoot);

                for (int j = 0; j < heart.transform.childCount; j++)
                {
                    var halfHeart = heart.transform.GetChild(j).gameObject;
                    halfHeart.SetActive(false);
                    _hearts.Add(halfHeart);
                }
            }
        }
        else if (requiredSize < currentSize)
        {
            for (var i = requiredSize; i < currentSize; i++)
            {
                var emptyHeart = _emptyHearts[i];
                _emptyHearts.Remove(emptyHeart);
                Destroy(emptyHeart);

                var halfHeart1 = _hearts[i * 2];
                _hearts.Remove(halfHeart1);
                Destroy(halfHeart1);

                var halfHeart2 = _hearts[(i * 2) - 1];
                _hearts.Remove(halfHeart2);
                Destroy(halfHeart2);                
            }
        }
    }

    public void Unload()
    {
        foreach (var slot in _weaponSlots)
        {
            Destroy(slot.Item1.gameObject);
            Destroy(slot.Item2.gameObject);
        }

        _weaponSlots.Clear();
    }
}
