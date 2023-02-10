using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;

    private int _run = Animator.StringToHash("Run");
    private int _victory = Animator.StringToHash("Victory");
    private int _punch = Animator.StringToHash("Punch");
    private int _startShoot = Animator.StringToHash("Shoot");

    private void Start() => _animator = GetComponent<Animator>();

    public void Run()
    {
        _animator.SetBool(_startShoot, false);
        _animator.SetBool(_run, true);
        _animator.SetBool(_victory, false);
    }

    public void Victory()
    {
        _animator.SetBool(_run, false);
        _animator.SetBool(_startShoot, false);
        _animator.SetBool(_punch, false);
        _animator.SetBool(_victory, true);
    }

    public void Punch()
    {
        _animator.SetBool(_run, false);
        _animator.SetBool(_punch, true);
    }

    public void Shoot()
    {
        _animator.SetBool(_run, false);
        _animator.SetBool(_startShoot, true);
    }

    public void Disable() => _animator.enabled = false;

    public void UseFreezeSpeed() => _animator.speed = 0.5f;
}