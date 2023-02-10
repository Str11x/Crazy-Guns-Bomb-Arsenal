using System.Collections;
using UnityEngine;

public class Prisoner : Citizen
{
    private Vector3 _direction = Vector3.forward;
    private int _isFree = Animator.StringToHash("IsFree");
    private bool _isFinish;
    private void Start() => StartCoroutine(Run());

    protected new IEnumerator Run()
    {
        while (_isFinish != true)
        {
            transform.Translate(_direction * _speed * Time.deltaTime);
            yield return _fixedUpdate;
        }

        _animator.SetBool(_isFree, true);
    }

    private void OnTriggerEnter(Collider other) => _isFinish = true;
}