using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public GameObject root;

    [Header("Weapons")]
    public Transform weaponsRoot;
    private List<WeaponSlot> _weaponSlots = new List<WeaponSlot>();
    public WeaponSlot weaponSlotPrefab;

    [Header("Health")]
    public Transform emptyHeartsRoot;
    public GameObject emptyHeartPrefab;
    private List<GameObject> _emptyHearts = new List<GameObject>();

    public Transform heartsRoot;
    public GameObject heartPrefab;
    private List<GameObject> _heats = new List<GameObject>();

    public void InitWeapons()
    {

    }

    public void AddWeaponSlot()
    {
        if (_weaponSlots.Count < 5)
        {
            var weaponSlot = Instantiate(weaponSlotPrefab, weaponsRoot);
            if (_weaponSlots.Count != 0)
                weaponSlot.gameObject.SetActive(false);
            _weaponSlots.Add(weaponSlot);
        }
    }

    public void SetWeapons(Texture[] weaponTextures)
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            _weaponSlots[i].imagePlaceholder.texture = weaponTextures[i];
            _weaponSlots[i].imagePlaceholder.gameObject.SetActive(true);
        }
    }

    public void SetHealth(int health)
    {
        foreach (var heart in _heats)
            heart.SetActive(false);

        for (int i = 0; i < health; i++)
        {
            _heats[i].SetActive(true);
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
                    _heats.Add(halfHeart);
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

                var halfHeart1 = _heats[i * 2];
                _heats.Remove(halfHeart1);
                Destroy(halfHeart1);

                var halfHeart2 = _heats[(i * 2) - 1];
                _heats.Remove(halfHeart2);
                Destroy(halfHeart2);                
            }
        }
    }

    public void Unload()
    {
        foreach (var weaponSlot in _weaponSlots)
            Destroy(weaponSlot.gameObject);

        _weaponSlots.Clear();
    }
}
