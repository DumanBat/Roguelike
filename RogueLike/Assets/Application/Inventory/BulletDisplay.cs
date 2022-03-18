using UnityEngine;
using UnityEngine.UI;

public class BulletDisplay : MonoBehaviour
{
    private Image _emptyBulletImage;
    private Image _bulletImage;

    private void Awake()
    {
        _emptyBulletImage = GetComponent<Image>();
        _bulletImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetImage(Sprite bulletSprite)
    {
        _emptyBulletImage.sprite = bulletSprite;
        _bulletImage.sprite = bulletSprite;
    }

    public void SetActiveBullet(bool val)
    {
        _bulletImage.gameObject.SetActive(val);
    }
}
