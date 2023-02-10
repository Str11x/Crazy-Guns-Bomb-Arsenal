using System.Collections;
using UnityEngine;

public class ArrowFlyAnimation : MonoBehaviour
{
    [SerializeField] private float _animationRange;
    [SerializeField] private float _animationSpeed = 0.75f;
    [SerializeField] private Transform[] _points;

    private int _currentPoint;
    private int _minDistance = 2;
    private Vector3 _target;

    private void Start()
    {
        transform.position = _points[_currentPoint + 1].transform.position;
        _target = _points[_currentPoint].transform.position;
        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        while(gameObject.activeSelf != false)
        {
            Vector3 point = Vector3.MoveTowards(transform.position, _target, _animationSpeed);
            transform.position = point;

            if (Vector3.Distance(transform.position, _target) < _minDistance)
            {
                if (_currentPoint + 1 >= _points.Length)
                {
                    _currentPoint = 0;
                    _target = _points[_currentPoint].transform.position;
                }

                else
                {
                    _target = _points[++_currentPoint].transform.position;
                }                    
            }

            yield return null;           
        }
    }
}