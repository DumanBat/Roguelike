using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public abstract class Weapon : MonoBehaviour, IPickable
{
    protected string weaponName;

    protected int damage;

    protected float bpm;
    protected float bulletVelocity;
    protected float timeBetweenShooting;

    protected int magazineSize;
    protected int bulletsPerShot;

    protected int bulletsLeft;
    protected int bulletsShot;

    protected bool shooting;
    protected bool readyToShot;
    protected bool reloading;

    protected ObjectPool<Bullet> bulletPool;
    public Bullet bulletPrefab;

    public RawImage weaponImage;
    public Transform spritesTransform;
    public GameObject[] weaponSprites;
    public BoxCollider2D weaponCollider;

    public Action<Weapon> onWeaponPickUp;

    public virtual void Init()
    {
        weaponCollider = GetComponent<BoxCollider2D>();
        weaponSprites = new GameObject[spritesTransform.childCount];

        for (int i = 0; i < spritesTransform.childCount; i++)
            weaponSprites[i] = spritesTransform.GetChild(i).gameObject;

        bulletPool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(bulletPrefab);
        }, bullet => {
            bullet.gameObject.SetActive(true);
        }, bullet => {
            bullet.gameObject.SetActive(false);
        }, bullet => {
            Destroy(bullet.gameObject);
        },
        false, magazineSize, magazineSize * 2);
    }

    public virtual void Shot(Vector2 direction)
    {
        var bullet = bulletPool.Get();
        var velocity = direction.normalized * bulletVelocity;

        //kostyl for shot initial position
        var position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        bullet.Shot(position, direction, velocity);
    }

    public virtual void Reload()
    {

    }

    public void PickUp()
    {
        onWeaponPickUp.Invoke(this);
    }
}
