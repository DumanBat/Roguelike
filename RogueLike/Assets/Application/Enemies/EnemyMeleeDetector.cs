using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDetector : MonoBehaviour
{
    private CircleCollider2D _meleeColider;
    private string _enemyTag;

    public bool enemyInMeleeRange = false;

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
            enemyInMeleeRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            enemyInMeleeRange = false;
        }
    }
}
