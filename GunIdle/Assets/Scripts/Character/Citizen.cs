using System.Collections;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] private Level _levelService;
    [SerializeField] protected float _speed = 0.1f;
    [SerializeField] protected Transform _target;

    protected int _endDistance = 2;
    private int _isRun = Animator.StringToHash("IsFail");
    protected WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    private void OnEnable() => _levelService.LevelFailed += StartRun;

    private void OnDisable()
    {
        StopAllCoroutines();
        _levelService.LevelFailed -= StartRun;
    }

    private void StartRun()
    {
        _animator.SetBool(_isRun, true);
        StartCoroutine(Run());
    }

    protected IEnumerator Run()
    {
        while(Vector3.SqrMagnitude(transform.position - _target.position) > _endDistance)
        {
            transform.Translate(_target.position * _speed * Time.deltaTime);
            transform.LookAt(_target);
            yield return _fixedUpdate;
        }
    }
}