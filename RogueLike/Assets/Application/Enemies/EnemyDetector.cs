using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    /// <summary>
    /// TODO:
    /// Remove PlayerController class from on trigger enter - DONE, replaced with IDamageable
    /// </summary>
    [SerializeField]
    private string _enemyTag;

    private CircleCollider2D _areaCollider;
    public EnemyMeleeDetector _meleeDetector;
    private float _aggroCooldown;

    public IDamageable detectedTarget;
    public bool EnemyInRange => detectedTarget != null;

    private void Awake()
    {
        _areaCollider = GetComponent<CircleCollider2D>();

        _meleeDetector = GetComponentInChildren<EnemyMeleeDetector>();
    }

    public void Init(float aggroRange, float meleeRange, float aggroCooldown)
    {
        _areaCollider.radius = aggroRange;
        _meleeDetector.Init(_enemyTag, meleeRange);
        _aggroCooldown = aggroCooldown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            detectedTarget = collision.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            detectedTarget = collision.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            StartCoroutine(ClearDetectedTargetAfterDelay());
        }
    }

    private IEnumerator ClearDetectedTargetAfterDelay()
    {
        yield return new WaitForSeconds(_aggroCooldown);
        detectedTarget = null;
    }
}
