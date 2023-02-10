using System.Collections;
using UnityEngine;

public class BombRigidbody : MonoBehaviour
{
    [SerializeField] private Bomb _bombService;
    [SerializeField] private float _rotateStep = 2;

    private float _nextRotation;
    private bool _isGrounded;
    private Coroutine _RotateMoving;

    private void OnCollisionEnter(Collision collision)
    {
            _isGrounded = true;
            StopCoroutine(_RotateMoving);
            _bombService.Activate();  
    }

    private void OnEnable() => _RotateMoving = StartCoroutine(RotateFly());

    private IEnumerator RotateFly()
    {
        while(_isGrounded == false)
        {         
            transform.rotation = Quaternion.Euler(0, 0, _nextRotation);
            _nextRotation += _rotateStep;
            yield return null;
        }
    }
}