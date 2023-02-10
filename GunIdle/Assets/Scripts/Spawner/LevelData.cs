using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SpawnerData", order = 51)]
public class LevelData : ScriptableObject
{
    [Header("Generate From Data")]
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Enemy _newEnemy;
    [SerializeField] private Enemy _boss;
    [SerializeField] private int _count;
    [SerializeField] private int _soldiersCount;
    [SerializeField] private int _newEnemiesCount;
    [SerializeField] private float _minOffsetZ;
    [SerializeField] private float _maxOffsetZ;
    [SerializeField] private float _bossSpeed;
    [SerializeField] private int _bossCount;

    [Header("Autogenerate")]
    [SerializeField] private List<Enemy> _enemies;

    public Enemy GetEnemy()
    {
        return _enemy;
    }  

    public Enemy GetNewEnemy()
    {
        return _newEnemy;
    }

    public Enemy GetBoss()
    {
        return _boss;
    }

    public int GetEnemiesCount()
    {
        return _count;
    }

    public int GetSoldiersCount()
    {
        return _soldiersCount;
    }

    public int GetNewEnemiesCount()
    {
        return _newEnemiesCount;
    }

    public float GetMinOffsetZ()
    {
        return _minOffsetZ;
    }

    public float GetMaxOffsetZ()
    {
        return _maxOffsetZ;
    }

    public float GetBossSpeed()
    {
        return _bossSpeed;
    }

    public int GetBossCount()
    {
        return _bossCount;
    }
}