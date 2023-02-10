using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponOverheatingDisplay : MonoBehaviour
{
    [SerializeField] private List <GunForward> gunForward = new List<GunForward>();
    [SerializeField] private Image _bar;
    [SerializeField] private CanvasGroup _display;
    [SerializeField] private float _colorChangeSpeed = .01f;
    [SerializeField] private float _changeThreshold = .7f;
    [SerializeField] private AudioSource _heating;
    [SerializeField] private AudioSource _heatingStop;
    [SerializeField] private Level _level;

    private int _maxDegree = 1;

    private void OnEnable()
    {
        _level.LevelCompleted += Disable;
        _level.LevelFailed += Disable;
        _level.Started += Enable;

        foreach (GunForward gunForward in gunForward)
            gunForward.HeatValueChanged += UpdateRender;
    }

    private void OnDisable()
    {
        _level.LevelCompleted -= Disable;
        _level.LevelFailed -= Disable;
        _level.Started -= Enable;

        foreach (GunForward gunForward in gunForward)
            gunForward.HeatValueChanged -= UpdateRender;
    }

    private void Disable()
    {
        _heating.Stop();
        gameObject.SetActive(false);
    }
    private void Enable() => _display.alpha = 1;

    private void UpdateRender(float value, float degreeHeating)
    {       
        if(_bar.fillAmount > _changeThreshold)
            _bar.color = Color.Lerp(_bar.color, Color.red * degreeHeating, _colorChangeSpeed);
        else
            _bar.color = Color.Lerp(_bar.color, Color.yellow * value, _colorChangeSpeed);

        _bar.fillAmount = degreeHeating;
        PlayHeatingSound(degreeHeating);
    }

    private void PlayHeatingSound(float degree)
    {
        _heating.volume = degree;

        if (_heating.isPlaying != true)
            _heating.Play();

        if (degree == _maxDegree && _heatingStop.isPlaying != true)
            _heatingStop.Play();
    }
}