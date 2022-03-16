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

    private Weapon _prefabHandler;
    public bool isAuto;

    private int _damage;
    private float _bpm; // converts to float on Init()
    private int _magazineSize;
    private int _bulletsPerShot;

    private float _bulletVelocity;
    private float _reloadingDuration;

    private int _bulletsLeft;

    public WeaponFactory OriginFactory { get; set; }
    private float _lastFired = -9999;

    protected bool readyToShot
    {
        get
        {
            if (reloading == false && _bulletsLeft > 0)
                return true;
            else
                return false;
        }
    }
    protected bool reloading;
    public bool IsReloading() => reloading;
    public void StopReloading() => reloading = false;

    protected ObjectPool<Bullet> bulletPool;
    public Bullet bulletPrefab;
    public RawImage bulletImage;

    public RawImage weaponImage;
    public SpriteRenderer weaponInGameSprite;
    public Transform spritesTransform;
    public GameObject[] weaponSprites;
    public BoxCollider2D weaponCollider;

    public Action onWeaponPickUp;
    public Action<Weapon> onAddedToInventory;
    public void PickUp()
    {
        onWeaponPickUp?.Invoke();
        onAddedToInventory?.Invoke(this);
    }

    public Transform _firepoint;
    public void SetFirepoint(Transform point) => _firepoint = point;

    public virtual void Init(WeaponConfig config)
    {
        _prefabHandler = config.weaponPrefab;

        isAuto = config.IsAuto;
        _damage = config.Damage;
        _bpm = 60.0f / config.BPM;
        _magazineSize = config.MagazineSize;
        _bulletsPerShot = config.BulletsPerShot;
        _bulletVelocity = config.BulletVelocity;
        _reloadingDuration = config.ReloadingDuration;

        _bulletsLeft = config.BulletsLeft;

        weaponCollider = GetComponent<BoxCollider2D>();
        weaponSprites = new GameObject[spritesTransform.childCount];

        for (int i = 0; i < spritesTransform.childCount; i++)
            weaponSprites[i] = spritesTransform.GetChild(i).gameObject;

        bulletPool = new ObjectPool<Bullet>(() =>
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.Init(bulletPool, _damage);
            return bullet;
        }, bullet => {
            bullet.gameObject.SetActive(true);
        }, bullet => {
            bullet.gameObject.SetActive(false);
        }, bullet => {
            Destroy(bullet.gameObject);
        },
        false, _magazineSize, _magazineSize * 2);
    }

    public virtual bool Shot(Vector2 direction)
    {
        if (!readyToShot) return false;

        if (Time.time - _lastFired > _bpm)
        {
            _lastFired = Time.time;
            ShotBullet(direction);
            _bulletsLeft--;
            return true;
        }

        return false;
    }

    public virtual void ShotBullet(Vector2 direction)
    {
        var bullet = bulletPool.Get();
        var velocity = direction.normalized * _bulletVelocity;

        var position = new Vector3(_firepoint.position.x, _firepoint.position.y, _firepoint.position.z);
        bullet.Shot(position, direction, velocity);
    }

    public virtual IEnumerator Reload()
    {
        if (_bulletsLeft == _magazineSize)
            yield break;
        if (reloading)
            yield break;

        reloading = true;
        _weaponController.onReload.Invoke(_reloadingDuration);
        yield return new WaitForSeconds(_reloadingDuration);
        _bulletsLeft = _magazineSize;
        reloading = false;
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

    public WeaponConfig GetConfig()
    {
        WeaponConfig config = new WeaponConfig()
        {
            weaponPrefab = _prefabHandler,
            IsAuto = isAuto,
            Damage = _damage,
            BPM = Mathf.RoundToInt(60.0f / _bpm),
            MagazineSize = _magazineSize,
            BulletsPerShot = _bulletsPerShot,
            BulletVelocity = _bulletVelocity,
            ReloadingDuration = _reloadingDuration,

            BulletsLeft = _bulletsLeft
        };

        return config;
    }
}
