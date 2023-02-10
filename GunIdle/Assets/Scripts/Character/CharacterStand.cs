using System;
using UnityEngine;

public class CharacterStand : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private int _shield = 100;
    [SerializeField] private WarningDisplay _lowHPDisplay;
    [SerializeField] private WarningDisplay _OverheatingDisplay;
    [SerializeField] private ParticleSystem _damageEffect;
    [SerializeField] private ParticleSystem _damageTurretEffect;
    [SerializeField] private AudioSource _airHit;
    [SerializeField] private Level _level;

    private float _heightOffset = 1;
    private float _lowHPTreshold = 0.2f;
    private int _maxHealth;
    private int _maxShield;
    private bool _isShieldActivated;
    private float _citizenBuffToHealth = 10;

    public bool IsLowHPReached { get; private set; }

    public event Action HealthIsOver;
    public event Action<int, int> HealthChanged;
    public event Action<int, int> ShieldChanged;
    public event Action ShieldBroken;
    public event Action StopHealthCalculate;
    public event Action <int> HPRecalculated;

    public Vector3 Position => new Vector3 (transform.position.x, _heightOffset, transform.position.z);

    private void Start()
    {
        CalculateHP();
        _level.LevelCompleted += StopHealthCalculate;
        _maxHealth = _health;
        _maxShield = _shield;
    }

    private void OnEnable()
    {
        _level.LevelCompleted += StopHealthCalculate;
        _level.LevelFailed += StopHealthCalculate;
    }

    private void OnDisable()
    {
        _level.LevelCompleted -= StopHealthCalculate;
        _level.LevelFailed -= StopHealthCalculate;
    }


    public void ActivateShield() => _isShieldActivated = true;

    public void ApplyDamage(int damage)
    {
        if (_isShieldActivated)
            DoDamageToShield(damage);       
        else if(_health > 0)
            DoDamageToHealth(damage);             
    }
    
    public void SoundHit() => _airHit.Play();

    private void CalculateHP()
    {
        int freeCitizensCount = PlayerPrefs.GetInt(KeySave.FREE_CITIZENS);

        if (freeCitizensCount > 0)
        {
            _health += (int)(_health * freeCitizensCount / _citizenBuffToHealth);
            HPRecalculated?.Invoke(freeCitizensCount);
        }          
    }

    private void ChangeStateLowHPDisplay()
    {
        if(_health < _maxHealth * _lowHPTreshold && _OverheatingDisplay.enabled != true)
        {
            IsLowHPReached = true;
            _lowHPDisplay.Show();
        }
        else
        {
            _lowHPDisplay.Hide();
        }
    }

    private void DoDamageToShield(int damage)
    {
        _shield -= damage;

        if (_shield <= 0)
        {
            _shield = 0;
            _isShieldActivated = false;
            ShieldBroken?.Invoke();
        }            

        ShieldChanged?.Invoke(_shield, _maxShield);
        _damageEffect.Play();
        _damageTurretEffect.Play();
    }

    private void DoDamageToHealth(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            HealthChanged?.Invoke(_health, _maxHealth);
            HealthIsOver?.Invoke();
        }
        else
        {
            HealthChanged?.Invoke(_health, _maxHealth);
            _damageEffect.Play();
            _damageTurretEffect.Play();
            ChangeStateLowHPDisplay();
        }      
    }

    private void StopHealthCalcuate() => StopHealthCalculate?.Invoke();
}