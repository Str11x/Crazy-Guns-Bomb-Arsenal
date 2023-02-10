using UnityEngine;

public class EnemyHealthCalculation
{
    private float _defaultValue = 100;
    private float _difficultyFactor = 0.090f;
    private int _weaponDifficultyFactor = 10;
    private float _health;
    private float _bossHealth;
    private float _bossHealthDifficult = 7;
    private int _weaponInitialNumber = 1;
    private int _growDifficultThreshold = 10;
    private int _stepDifficulty = 5;
    private float _golemDifficult = 1.2f;
    private int _endGame = 100;
    private int _autoLevel = 100;
    private int _autoLevelDifficult = 3;
    private int _maxBossHealth = 50000;
    private int _maxHealth = 30000;
    private int _minHealth = 0;

    private int CalculateDifficult()
    {
        int levelNumber = PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER);

        if (levelNumber >= _growDifficultThreshold)
            _weaponDifficultyFactor += _stepDifficulty * levelNumber / _growDifficultThreshold;

        int weaponDifficulty = (PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER) + _weaponInitialNumber) * _weaponDifficultyFactor;

        return weaponDifficulty;
    }

    public int GetValue(bool isGolem)
    {
        int stamina = PlayerPrefs.GetInt(KeySave.LEVEL_STAMINA);
        int fireRate = PlayerPrefs.GetInt(KeySave.LEVEL_FIRE_RATE);
        int reward = PlayerPrefs.GetInt(KeySave.LEVEL_REWARD);

        int weaponPower = stamina + fireRate + reward;
        _health = _defaultValue * ((CalculateDifficult() + weaponPower) * _difficultyFactor);

        if (isGolem)
            _health = _health * _golemDifficult;

        if(PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) >= _autoLevel)
            _health = _health * _autoLevelDifficult;

        return Mathf.Clamp((int)_health, _minHealth, _maxHealth); ;
    }

    public int GetBossValue(bool isGolem)
    {
        if (PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) >= _endGame)
        {
            _bossHealth = _health * _bossHealthDifficult + CalculateDifficult() * _stepDifficulty;
            return Mathf.Clamp((int)_bossHealth, _minHealth, _maxBossHealth);
        }
        
        _bossHealth = _health * _bossHealthDifficult + CalculateDifficult();

        if (isGolem)
            _bossHealth = _bossHealth * _golemDifficult;

        if (PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) >= _autoLevel)
            _bossHealth = _bossHealth * _autoLevelDifficult;

        return Mathf.Clamp((int)_bossHealth, _minHealth, _maxBossHealth);
    }
}