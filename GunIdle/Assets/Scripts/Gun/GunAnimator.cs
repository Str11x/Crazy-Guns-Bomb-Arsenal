using UnityEngine;

public class GunAnimator : MonoBehaviour
{
    [SerializeField] private int _shootingSpeed = 3;
    [SerializeField] private int _idleSpeed = 1;

    private Animator _animator;

    private const string SHOOT = "Shoot";

    private void Awake() => _animator = GetComponent<Animator>();

    public void Idle()
    {
        _animator.SetBool(SHOOT, false);
        ChangeAnimatorSpeed(_idleSpeed);
    }

    public void Shoot()
    {
        _animator.SetBool(SHOOT, true);
        ChangeAnimatorSpeed(_shootingSpeed);
    } 

    public void Deactivate() => _animator.enabled = false;

    private void ChangeAnimatorSpeed(int speed) => _animator.speed = speed;
}