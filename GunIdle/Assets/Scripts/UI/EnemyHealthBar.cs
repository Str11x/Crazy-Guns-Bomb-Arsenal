using UnityEngine;

public class EnemyHealthBar : HealthBar
{
    [SerializeField] private Enemy _enemy;

    private void OnEnable()
    {
        _enemy.HealthChanged += OnChangeHealth;
        _enemy.Died += Hide;
    }

    private void OnDisable()
    {    
        _enemy.HealthChanged -= OnChangeHealth;
        _enemy.Died -= Hide;
    }

    private void Start() => Hide();

    private void Hide(Enemy enemy) => Hide();
}