using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(AbilityResponse))]
[RequireComponent(typeof(EnemyMove))]
public class EnemyHitbox : MonoBehaviour
{   
    private Enemy _enemy;
    private AbilityResponse _enemyAbilityReaction;
    private EnemyMove _enemyMove;
    private int _health;
    private float _speed;
    private readonly int _minHealth = 0;
    private int _maxHealth;
    
    public bool IsExplosiveForce { get; private set; }
    public bool IsFreezing { get; private set; }

    private void Awake()
    {
        _enemyAbilityReaction = GetComponent<AbilityResponse>();
        _enemy = GetComponent<Enemy>();
        _enemyMove= GetComponent<EnemyMove>();
    }

    public void SetHealth(int health, int maxHealth)
    {
        _health = health;
        _maxHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        PlayEffects();

        _health = Mathf.Clamp(_health - damage, _minHealth, _maxHealth);
        _enemy.HealthChanged?.Invoke(_health, _maxHealth);

        if (_health == _minHealth)
            _enemy.Die();
    }

    public void ApplyDamage(Bomb bomb)
    {
        IsExplosiveForce = true;

        if (_enemyAbilityReaction.IsCurrentAbilityDeadly(bomb))
        {
            _enemy.Die();          
        }
        else if (_enemyAbilityReaction.BombReaction == true && _enemyAbilityReaction.IsBoss != true)
        {
            _health = Mathf.Clamp(_health - (int)(_maxHealth * _enemyAbilityReaction.FireBombPower), _minHealth, _maxHealth);
            _enemy.HealthChanged?.Invoke(_health, _maxHealth);
            PlayEffects();
        }
        else if (_enemyAbilityReaction.IsBoss == true)
        {
            _health = Mathf.Clamp(_health - (int)(_maxHealth * _enemyAbilityReaction.BossBombPower), _minHealth, _maxHealth);
            _enemy.HealthChanged?.Invoke(_health, _maxHealth);
            PlayEffects();        
        }

        if (_health == _minHealth)
            _enemy.Die();
    }

    public void ApplyDamage(Freeze bomb)
    {
        IsExplosiveForce = true;
        IsFreezing = true;

        if (_enemyAbilityReaction.IsCurrentAbilityDeadly(bomb))
        {
            _enemy.Die();
        }
        else if (_enemyAbilityReaction.IsFreezeBombReaction == true && _enemyAbilityReaction.IsBoss != true)
        {
            _health = Mathf.Clamp(_health - (int)(_maxHealth * _enemyAbilityReaction.FreezeBombPower), _minHealth, _maxHealth);
            _enemy.HealthChanged?.Invoke(_health, _maxHealth);
            _enemyMove.SetSpeed(_enemyMove.Speed * _enemyAbilityReaction.SpeedInFreeze);
            PlayEffects();
        }
        else if (_enemyAbilityReaction.IsBoss == true && _enemyAbilityReaction.IsFreezeBombReaction == true)
        {
            _enemy.SetSpeed(_speed * _enemyAbilityReaction.SpeedInFreeze);
            PlayEffects();
        }

        if (_health == _minHealth)
            _enemy.Die();
    }

    public void SetCurrentBossSpeed(float speed) => _speed = speed;

    private void PlayEffects() => _enemy.PlayDamageEffects();
}