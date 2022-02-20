using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public abstract class Weapon : MonoBehaviour, IPickable
{
    protected string weaponName;
    private WeaponController _weaponController;

    protected int damage;

    public bool isAuto = false;
    protected float bpm;
    private float _lastFired = -9999;
    protected float bulletVelocity;
    protected float timeBetweenShooting;

    protected int magazineSize;
    protected int bulletsPerShot;

    protected int bulletsLeft;
    protected int bulletsShot;

    protected bool shooting;
    protected bool readyToShot
    {
        get
        {
            if (reloading == false && bulletsLeft > 0)
                return true;
            else
                return false;
        }
    }

    protected bool reloading;
    protected float reloadingDuration;

    protected ObjectPool<Bullet> bulletPool;
    public Bullet bulletPrefab;

    public RawImage weaponImage;
    public SpriteRenderer weaponInGameSprite;
    public Transform spritesTransform;
    public GameObject[] weaponSprites;
    public BoxCollider2D weaponCollider;

    public Action<Weapon> onWeaponPickUp;

    public Transform _firepoint;

    public virtual void Init()
    {
        weaponCollider = GetComponent<BoxCollider2D>();
        weaponSprites = new GameObject[spritesTransform.childCount];

        for (int i = 0; i < spritesTransform.childCount; i++)
            weaponSprites[i] = spritesTransform.GetChild(i).gameObject;

        bulletPool = new ObjectPool<Bullet>(() =>
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.Init(bulletPool, damage);
            return bullet;
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
        if (readyToShot)
        {
            if (Time.time - _lastFired > 1 / bpm)
            {
                _lastFired = Time.time;
                ShotBullet(direction);
                bulletsLeft--;
            }
        }
    }

    public virtual void ShotBullet(Vector2 direction)
    {
        var bullet = bulletPool.Get();
        var velocity = direction.normalized * bulletVelocity;

        var position = new Vector3(_firepoint.position.x, _firepoint.position.y, _firepoint.position.z);
        bullet.Shot(position, direction, velocity);
    }

    public virtual IEnumerator Reload()
    {
        if (bulletsLeft == magazineSize)
            yield break;
        if (reloading)
            yield break;

        reloading = true;
        _weaponController.onReload.Invoke(reloadingDuration);
        yield return new WaitForSeconds(reloadingDuration);
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public void StopReloading()
    {
        reloading = false;
    }

    public void PickUp()
    {
        Debug.LogWarning("weapon pick up - " + gameObject.name);
        onWeaponPickUp.Invoke(this);
    }

    public void SetFirepoint(Transform point)
    {
        _firepoint = point;
    }

    public bool IsReloading()
    {
        return reloading;
    }

    public void AddToInventory(Transform weaponsRoot)
    {
        Destroy(weaponCollider);
        transform.SetParent(weaponsRoot);
        transform.SetSiblingIndex(weaponsRoot.childCount - 2);
        transform.localPosition = Vector3.zero;
        weaponInGameSprite.gameObject.SetActive(false);

        _weaponController = GetComponentInParent<WeaponController>();
    }
}
