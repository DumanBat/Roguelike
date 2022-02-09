using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDetector : MonoBehaviour
{
    private CircleCollider2D _meleeColider;
    private string _enemyTag;

    public Action onMeleeDetected;
    public Action onMeleeRangeLeft;

    private void Awake()
    {
        _meleeColider = GetComponent<CircleCollider2D>();
    }

    public void Init(string enemyTag, float meleeRange)
    {
        _enemyTag = enemyTag;
        _meleeColider.radius = meleeRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            onMeleeDetected?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            onMeleeRangeLeft?.Invoke();
        }
    }
}
