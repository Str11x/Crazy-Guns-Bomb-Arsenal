using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class GunForward : MonoBehaviour
{
    [SerializeField] private Stamina _upgrade;
    [SerializeField] private Level _level;
    [SerializeField] private float _timeDoHeating;
    [SerializeField] private float _speedHeating;
    [SerializeField] private float _speedCooling;
    [SerializeField] private float _boardDoShowWarningDisplay;
    [SerializeField] private WarningDisplay _warningDisplay;
    [SerializeField] private ParticleSystem _smoke;

    private readonly float _maxHeatValue = 3f;
    private readonly float _minHeatValue = 0f;
    private float _intensity;
    private float _speedChange;
    private float _degreeHeating;
    private float _heatingDifficultySpeed = 0.7f;

    public bool IsMaxHeatReached { get; private set; }
    private bool _isHeat;
    private float _currentTime;
    private MeshRenderer _meshRenderer;
    private Coroutine _coroutine;
    private Gun _gun;

    public event Action <float, float> HeatValueChanged;

    private void Start()
    {
        _gun = GetComponentInParent<Gun>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _intensity = _minHeatValue;
    }

    private void OnEnable() => _upgrade.ValueChanged += OnChangeValue;

    private void OnDisable() => _upgrade.ValueChanged -= OnChangeValue;

    public void ChangeState(bool flag)
    {
        if (flag)
        {
            if (_intensity != _minHeatValue || _isHeat)
            {
                Heat();
                return;
            }

            if (!_isHeat)
                Invoke(nameof(Heat), _timeDoHeating);
        }
        else
        {
            CancelInvoke();
            Cooling();
        }
    }

    private IEnumerator ChangeHeat(float value)
    {
        if (_intensity == _minHeatValue)
            _smoke.Stop();

        ChangeGunAvailability();

        while (_intensity != value)
        {
            _intensity = Mathf.MoveTowards(_intensity, value, _speedChange * Time.deltaTime);
            _meshRenderer.material.SetColor("_EmissionColor", Color.red * _intensity);
            _degreeHeating = _intensity / _maxHeatValue; 
            HeatValueChanged?.Invoke(_intensity, _degreeHeating);

            if (_intensity == _maxHeatValue)
                Cooling();

            if(_gun.IsLowHP() != true)
            {
                if (_intensity >= _boardDoShowWarningDisplay || IsMaxHeatReached)
                    _warningDisplay.Show();
                else
                    _warningDisplay.Hide();
            }
            
            yield return new WaitForFixedUpdate();
        }
    }

    private void ChangeGunAvailability()
    {
        if (_intensity == _maxHeatValue)
        {
            IsMaxHeatReached = true;
            _warningDisplay.Show();
        }
        else if(_intensity == _minHeatValue)
        {
            IsMaxHeatReached = false;
            _warningDisplay.Hide();
        }
    }

    private void Heat()
    {
        _smoke.Play();
        _isHeat = true;
        StopAllCoroutines();
        _speedChange = _speedHeating;
        StartCoroutine(ChangeHeat(_maxHeatValue));
    }

    private void Cooling()
    {
        StopAllCoroutines();
        _speedChange = _speedCooling;
        StartCoroutine(ChangeHeat(_minHeatValue));
    }

    private void OnChangeValue(float value) => _timeDoHeating = value * _heatingDifficultySpeed;
}