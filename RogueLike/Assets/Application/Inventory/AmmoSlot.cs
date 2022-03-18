using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSlot : MonoBehaviour
{
    private BulletDisplay _bullet;
    private List<BulletDisplay> _bullets = new List<BulletDisplay>();

    public void SetAmmoSlot(Sprite bulletSprite, int ammoCount)
    {
        var bulletObject = transform.GetChild(0).GetComponent<BulletDisplay>();
        bulletObject.SetImage(bulletSprite);

        _bullet = bulletObject;
        _bullets.Add(_bullet);

        for (int i = 0; i < ammoCount - 1; i++)
        {
            var bullet = Instantiate(_bullet, transform);
            _bullets.Add(bullet);
        }
    }

    public void SetBullets(int val)
    {
        if (val == _bullets.Count)
        {
            foreach (var bullet in _bullets)
                bullet.SetActiveBullet(true);
        }
        else
        {
            foreach (var bullet in _bullets)
                bullet.SetActiveBullet(false);

            for (int i = 0; i < val; i++)
                _bullets[i].SetActiveBullet(true);
        }
    }
}
