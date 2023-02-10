using Cinemachine;
using System.Collections;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private WonDisplay _wonDisplay;
    [SerializeField] private FailDisplay _failDisplay;
    [SerializeField] private ParticleSystem _wonEffect;
    [SerializeField] private AudioSource _sadSound;
    [SerializeField] private AudioSource _happySound;
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private CitizensPrison _citizensPrison;

    private float _currentCameraFOV;
    private float _minFOV = 60;
    private float _maxFOV = 50;
    private float _targetValue;
    private Transform _direction;

    private void OnEnable() => _citizensPrison.CitizenEnabled += ChangeDirection;

    private void OnDisable() => _citizensPrison.CitizenEnabled -= ChangeDirection;

    public void ShowFailDisplay(float moneyToSession)
    {
        if(_failDisplay.isActiveAndEnabled != true && _wonDisplay.isActiveAndEnabled != true)
        {
            StartCoroutine(Zoom());
            _sadSound.Play();
            _failDisplay.SetValue((int)moneyToSession);
            _failDisplay.Show();
        }       
    }

    public void ShowWonDisplay(float moneyToSession)
    {
        if (_wonDisplay.isActiveAndEnabled != true && _failDisplay.isActiveAndEnabled != true)
        {
            StartCoroutine(Zoom());
            Zoom();
            _wonEffect.Play();
            _happySound.Play();
            _wonDisplay.SetValue(moneyToSession);
            _wonDisplay.Show();
        }          
    }

    private void ChangeDirection(Citizen target) => _direction = target.transform;

    private IEnumerator Zoom()
    {
        _targetValue = - _zoomSpeed;
        _currentCameraFOV = _camera.m_Lens.FieldOfView;
        _camera.Follow = _direction;

        while (true)
        {
            _camera.m_Lens.FieldOfView += _targetValue;

            if (_camera.m_Lens.FieldOfView <= _maxFOV)
                _targetValue = _zoomSpeed;
            
            if(_camera.m_Lens.FieldOfView >= _minFOV)
                _targetValue = -_zoomSpeed;

            yield return null;
        }
    }
}