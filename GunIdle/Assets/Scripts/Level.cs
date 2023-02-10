using Agava.WebUtility;
using GameAnalyticsSDK;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private Reward _upgrade;
    [SerializeField] private float _reward;   
    [SerializeField] private CharacterWallet _wallet;   
    [SerializeField] private float _timeToShowFailDisplay;   
    [SerializeField] private WeaponLevelCalculation _weaponLevelCalculation;
    [SerializeField] private CharacterStand _characterStand;
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private LevelEnd _levelEnd;

    [Header("Displays")]
    [SerializeField] private StartDisplay _startDisplay;
    [SerializeField] private EnemyDisplay _enemyDisplay;
    [SerializeField] private NewWeaponDisplay _newWeaponDisplay;
    [SerializeField] private GunShop _gunShop;

    private List<Enemy> _enemies = new List<Enemy>();
    private int _totalEnemys;
    private float _moneyToSession;
    private bool _isReadyToStart = true;
    private bool _isGameFinish;
    private Gun _currentGun;
    private string _currentLevel;

    public float StartTime { get; private set; }
    public int Number { get; private set; }

    public UnityAction<int, int> CountChange;
    public event Action LevelFailed;
    public event Action LevelCompleted;
    public event Action DamageDone;
    public event Action EnemyDie;
    public event Action Started;

    private void Awake()
    {   
        GameAnalytics.Initialize();
        Number = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        _characterStand.HealthIsOver += Fail;
        _weaponLevelCalculation.WeaponUpgraded += ShowNewWeaponDisplay;
        _weaponLevelCalculation.LevelUp += _spawner.ChangeHPValue;
        _upgrade.ValueChanged += OnChangeValue;
    }

    private void ShowNewWeaponDisplay() => _newWeaponDisplay.Enable();

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        _characterStand.HealthIsOver -= Fail;
        _weaponLevelCalculation.WeaponUpgraded -= ShowNewWeaponDisplay;
        _weaponLevelCalculation.LevelUp -= _spawner.ChangeHPValue;
        _upgrade.ValueChanged += OnChangeValue;
    }

    private void Start()
    {
        _currentLevel = PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER).ToString();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, _currentLevel.ToString());

        CountChange?.Invoke(_totalEnemys - _enemies.Count, _totalEnemys);
    }

    public void StartGame()
    {
        if (_isReadyToStart)
        {
            _enemyDisplay.Show();
            ChangeStateGame(true);
            _startDisplay.Hide();
            _currentGun.ChangeState(true);
            _isReadyToStart = false;
            Started?.Invoke();
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemy.SetReward(_reward);
        _enemies.Add(enemy);
        enemy.Died += ChangeCountAliveEnemy;
        enemy.DamageApplay += OnDamageApplay;
        _totalEnemys++;
    }

    public void Fail()
    {
        if(_isGameFinish != true)
        {
            _isReadyToStart = false;
            _levelEnd.ShowFailDisplay(_moneyToSession);
            ChangeStateGame(false);
            _currentGun.Die();
            LevelFailed?.Invoke();
            _isGameFinish = true;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, _currentLevel, (int)_moneyToSession);
        }      
    }

    public float GetReward()
    {
        return _reward;
    }

    public void ChangeCurrentGun(Gun newGun) => _currentGun = newGun;

    public void ShowGunShop() => _gunShop.gameObject.SetActive(true);

    private void ChangeCountAliveEnemy(Enemy enemy)
    {
        enemy.Died -= ChangeCountAliveEnemy;
        enemy.DamageApplay -= OnDamageApplay;
        _enemies.Remove(enemy);
        EnemyDie?.Invoke();
        CountChange?.Invoke(_totalEnemys - _enemies.Count, _totalEnemys);

        if (_enemies.Count == 0)
            Won();
    }

    private void Won()
    {
        if(_isGameFinish != true)
        {
            ChangeStateGame(false);
            LevelCompleted?.Invoke();
            _levelEnd.ShowWonDisplay(_moneyToSession);
            _isGameFinish = true;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, _currentLevel, (int)_moneyToSession);
        }
    }

    private void ChangeStateGame(bool flag)
    {
        foreach (var enemy in _enemies)
        {
            enemy.ChangeState(flag);
        }

        _currentGun.ChangeState(flag);
    }

    private void OnChangeValue(float value)
    {           
        _reward = (float)Math.Round(value, 1);

        foreach (var enemy in _enemies)
        {
            enemy.SetReward(_reward);
        }
    }

    private void OnDamageApplay()
    {
        _wallet.ApplyMoney(_reward);
        _moneyToSession += _reward;
        DamageDone?.Invoke();
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        AudioListener.pause = inBackground;
        AudioListener.volume = inBackground ? 0f : 1f;
    }
}