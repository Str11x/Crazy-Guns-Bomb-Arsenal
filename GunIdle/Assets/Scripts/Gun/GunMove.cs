using UnityEngine;

public class GunMove : MonoBehaviour
{
    [SerializeField] private GameObject _topGun;
    [SerializeField] private float _borderRotate;
    [SerializeField] private float _speedRotation = 1;
    [SerializeField] private float _gunHeight = 4;

    private bool _isMove = true;
    private float _yRotation = 0;
    private Camera _camera;

    private void Start() => _camera = Camera.main;

    public void ChangeState() => _isMove = false;

    private void Update()
    {
        if (_isMove)
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * _speedRotation);
            Vector3 difference = new Vector3(mousePosition.x, _gunHeight, 0) - _topGun.transform.position;

            _yRotation = Mathf.Clamp(difference.x, -_borderRotate, _borderRotate);
            difference = new Vector3(_yRotation, difference.y, difference.z);

            _topGun.transform.LookAt(difference);
        }
    }
}