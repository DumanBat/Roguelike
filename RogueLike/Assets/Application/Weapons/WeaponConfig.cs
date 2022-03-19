using System;
using UnityEngine;

[Serializable]
public class WeaponConfig
{
    public Weapon weaponPrefab;

    public bool IsAuto;
    [Range(1, 10)]
    public int Damage = 5;
    [Range(0, 900)]
    public int BPM = 5;
    [Range(1, 200)]
    public int MagazineSize = 30;
    [Range(1, 1000)]
    public int MaxTotalAmmoCapacity = 120;
    [Range(1, 10)]
    public int BulletsPerShot = 1;
    [Range(1f, 100.0f)]
    public float BulletVelocity = 10.0f;
    [Range(0.5f, 10.0f)]
    public float ReloadingDuration = 3.0f;

    [Header("RuntimeData")]
    public int MagazineAmmoLeft = 30;
    public int TotalAmmoLeft = 120;
}
