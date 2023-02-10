using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    protected EnemyAnimator EnemyAnimator;
    protected bool IsTargetReached;
    protected Coroutine MovementCoroutine;
    protected Enemy Enemy;
    protected bool _isActive;
    protected Vector3 _targetPath = Vector3.forward;
    protected Vector3 _directionMove = Vector3.forward;

    public float Speed => _speed;

    private void OnEnable()
    {
        EnemyAnimator = GetComponent<EnemyAnimator>();
        Enemy = GetComponent<Enemy>();
        Enemy.StateChanged += OnChangedState;
    }

    private void OnDisable() => Enemy.StateChanged -= OnChangedState;

    public void SetSpeed(float speed)
    {
        if (_speed != 0)
            _speed = speed;
    }

    public void SetNewTarget(Vector3 newPath)
    {
        _targetPath = newPath;
    }

    protected virtual IEnumerator Movement()
    {
        while (_isActive && IsTargetReached != true)
        {
            transform.Translate(_directionMove * _speed * Time.deltaTime);
            RotateToTarget();
            yield return null;
        }
    }

    protected void RotateToTarget()
    {
        if (_targetPath != Vector3.forward)
            transform.LookAt(_targetPath);
    }

    private void OnChangedState(bool flag)
    {
        _isActive = flag;

        if (_isActive)
        {
            MovementCoroutine = StartCoroutine(Movement());
            EnemyAnimator.Run();
        }
    }
}