using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    /// <summary>
    /// TODO:
    /// Remove PlayerController class from on trigger enter
    /// </summary>
    [SerializeField]
    private string _enemyTag;

    private CircleCollider2D _areaCollider;

    private EnemyMeleeDetector _enemyMeleeDetector;

    public Action onEnemyDetected;
    public Action onEnemyLeaveDetectionArea;

    public IDamageable detectedTarget;
    public bool EnemyInRange => detectedTarget != null;

    private void Awake()
    {
        _areaCollider = GetComponent<CircleCollider2D>();

        _enemyMeleeDetector = GetComponentInChildren<EnemyMeleeDetector>();
    }

    public void Init(float aggroRange, float meleeRange)
    {
        _areaCollider.radius = aggroRange;
        _enemyMeleeDetector.Init(_enemyTag, meleeRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            IDamageable attribute = collision.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;
            detectedTarget = attribute;
            Debug.Log("Detected target == null?");
            Debug.Log(detectedTarget == null);

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
        yield return new WaitForSeconds(3f);
        detectedTarget = null;
    }
}
