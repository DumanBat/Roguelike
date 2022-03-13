using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public GameObject root;
    public VerticalLayoutGroup weaponsLayoutGroup;
    public List<WeaponSlot> weaponSlots;

    public Transform emptyHeartsRoot;
    public GameObject emptyHeartPrefab;
    private List<GameObject> emptyHearts = new List<GameObject>();

    public Transform heartsRoot;
    public GameObject heartPrefab;
    private List<GameObject> hearts = new List<GameObject>();

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

    public void SetHealth(int health)
    {
        foreach (var heart in hearts)
            heart.SetActive(false);

        for (int i = 0; i < health; i++)
        {
            hearts[i].SetActive(true);
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
                emptyHearts.Add(emptyHeart);
                var heart = Instantiate(heartPrefab, heartsRoot);

                for (int j = 0; j < heart.transform.childCount; j++)
                {
                    var halfHeart = heart.transform.GetChild(j).gameObject;
                    halfHeart.SetActive(false);
                    hearts.Add(halfHeart);
                }
            }
        }
        else if (requiredSize < currentSize)
        {
            for (var i = requiredSize; i < currentSize; i++)
            {
                var emptyHeart = emptyHearts[i];
                emptyHearts.Remove(emptyHeart);
                Destroy(emptyHeart);

                var halfHeart1 = hearts[i * 2];
                hearts.Remove(halfHeart1);
                Destroy(halfHeart1);

                var halfHeart2 = hearts[(i * 2) - 1];
                hearts.Remove(halfHeart2);
                Destroy(halfHeart2);                
            }
        }
    }
}
