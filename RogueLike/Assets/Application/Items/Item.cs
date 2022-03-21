using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour, IPickable
{
    public ItemFactory OriginFactory { get; set; }

    private Item _prefabHandler;

    private bool _isEffect;
    private bool _isActivatable;
    protected int _value;

    public SpriteRenderer itemInGameSprite;
    public RawImage itemImage;
    public Action onPickUp;

    public virtual void Init(ItemConfig config)
    {
        _prefabHandler = config.ItemPrefab;

        _isEffect = config.IsEffect;
        _isActivatable = config.IsActivatable;
        _value = config.Value;
    }

    public abstract void Apply();

    public virtual void AddToInventory()
    {

    }

    public void PickUp()
    {
        if (_isActivatable)
        {
            AddToInventory();
            onPickUp.Invoke();
        }
        else
        {
            Apply();
            onPickUp.Invoke();
            Destroy(this.gameObject);
        }
    }
}
