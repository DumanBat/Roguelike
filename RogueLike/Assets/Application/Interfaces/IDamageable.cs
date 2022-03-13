using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int value);

    Vector2 GetPosition();
}
