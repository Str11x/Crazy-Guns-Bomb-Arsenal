using UnityEngine;

public class EnemiesCountGenerator
{
    private float _minEnemiesNumber = 1;
    private float _maxEnemiesNumber = 3;
    private float _maxBossCount = 4;

    public int GetEnemiesCount()
    {
        float count = Random.Range(_minEnemiesNumber, _maxEnemiesNumber);
        return (int)count;
    }

    public int GetBossCount()
    {
        float count = Random.Range(0, _maxBossCount);
        return (int)count;
    }
}