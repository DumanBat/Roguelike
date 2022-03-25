using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeHandler : MonoBehaviour
{
    private List<EnemyType> _bosses = new List<EnemyType>()
    {
        EnemyType.BossShark
    };

    private List<EnemyType> _enemies = new List<EnemyType>()
    {
        EnemyType.GreenJelly,
        EnemyType.PinkJelly,
        EnemyType.Snake,
        EnemyType.AutoSnake
    };
    public List<EnemyType> GetBossesTypes() => _bosses;
    public List<EnemyType> GetEnemiesTypes() => _enemies;
}
