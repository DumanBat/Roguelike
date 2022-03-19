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
    private int _maxTotalAmmoCapacity;
    private int _bulletsPerShot;

    private float _bulletVelocity;
    private float _reloadingDuration;

    private int _magazineAmmoLeft;
    private int _totalAmmoLeft;

    public int GetMagazineAmmoLeft() => _magazineAmmoLeft;
    public int GetMagazineSize() => _magazineSize;
    public int GetTotalAmmoLeft() => _totalAmmoLeft;
    public int GetMaxTotalAmmo() => _maxTotalAmmoCapacity;
    public WeaponFactory OriginFactory { get; set; }
    private float _lastFired = -9999;

    protected bool readyToShot
    {
        get
        {
            if (reloading == false && _magazineAmmoLeft > 0 && _totalAmmoLeft > 0)
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
        _maxTotalAmmoCapacity = config.MaxTotalAmmoCapacity;
        _bulletsPerShot = config.BulletsPerShot;
        _bulletVelocity = config.BulletVelocity;
        _reloadingDuration = config.ReloadingDuration;

        _magazineAmmoLeft = config.MagazineAmmoLeft;
        _totalAmmoLeft = config.TotalAmmoLeft;

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
            bullet.transform.rotation = Quaternion.identity;
            bullet.gameObject.SetActive(true);
        }, bullet => {
            bullet.gameObject.SetActive(false);
        }, bullet => {
            Destroy(bullet.gameObject);
        },
        false, _magazineSize, _magazineSize * 2);
    }

    public virtual bool Shot(Vector2 aimPos)
    {
        if (!readyToShot) return false;

        if (Time.time - _lastFired > _bpm)
        {
            _lastFired = Time.time;
            ShotBullet(aimPos);
            _magazineAmmoLeft--;
            _totalAmmoLeft--;
            return true;
        }

        return false;
    }

    public virtual void ShotBullet(Vector2 aimPos)
    {
        var bullet = bulletPool.Get();
        var direction = new Vector3(aimPos.x, aimPos.y, 0.0f) - _firepoint.position;
        var velocity = direction.normalized * _bulletVelocity;

        var position = new Vector3(_firepoint.position.x, _firepoint.position.y, _firepoint.position.z);
        bullet.Shot(position, direction, velocity);
    }

    public virtual IEnumerator Reload()
    {
        if (_magazineAmmoLeft == _magazineSize || _totalAmmoLeft == 0 || reloading)
            yield break;
        if (_magazineAmmoLeft == _totalAmmoLeft)
            yield break;

        reloading = true;
        _weaponController.onReload.Invoke(_reloadingDuration);
        yield return new WaitForSeconds(_reloadingDuration);
        _magazineAmmoLeft = _totalAmmoLeft < _magazineSize
            ? _totalAmmoLeft
            : _magazineSize;
        _weaponController.onBulletAmountChange.Invoke();
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
            MaxTotalAmmoCapacity = _maxTotalAmmoCapacity,
            BulletsPerShot = _bulletsPerShot,
            BulletVelocity = _bulletVelocity,
            ReloadingDuration = _reloadingDuration,

            MagazineAmmoLeft = _magazineAmmoLeft,
            TotalAmmoLeft = _totalAmmoLeft
        };

        return config;
    }
}
