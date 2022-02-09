using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField]
    private string _enemyTag;

    private Collider2D _areaCollider;

    private EnemyMeleeDetector _enemyMeleeDetector;
    private Collider2D _meleeCollider;

    public Action onEnemyDetected;
    public Action onEnemyLeaveDetectionArea;

    private void Awake()
    {
        _areaCollider = GetComponent<Collider2D>();

        _enemyMeleeDetector = GetComponentInChildren<EnemyMeleeDetector>();
        _meleeCollider = GetComponentInChildren<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            onEnemyDetected.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            onEnemyLeaveDetectionArea.Invoke();
        }
    }
}
