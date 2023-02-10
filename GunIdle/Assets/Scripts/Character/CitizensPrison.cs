using UnityEngine;
using System;
using TMPro;

public class CitizensPrison : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private Transform _prison;
    [SerializeField] private Citizen[] _citizens;
    [SerializeField] private GameObject[] _citizensRenderer;
    [SerializeField] private GameObject _congratulationWindow;
    [SerializeField] private SpriteRenderer [] _circles;
    [SerializeField] private int[] _releaseLevels;

    public event Action<Citizen> CitizenEnabled;

    public bool IsNewPrisionFree { get; private set; }
    private int _currentCitizensState;

    private void OnEnable() =>  _level.LevelCompleted += TryFreePrisoner;

    private void OnDisable() => _level.LevelCompleted -= TryFreePrisoner;

    private void Start()
    {
        _currentCitizensState = PlayerPrefs.GetInt(KeySave.FREE_CITIZENS);
        _citizensRenderer[_currentCitizensState].gameObject.SetActive(true);
        UpdateCircles();
    }

    private void UpdateCircles()
    {
        for (int i = 0; i < _circles.Length; i++)
        {
            if (i < _currentCitizensState)
                _circles[i].gameObject.SetActive(false);
            else
                _circles[i].gameObject.SetActive(true);
        }
    }
    private void TryFreePrisoner()
    {
        int currentLevel = PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) + 1;

        if (_currentCitizensState < _citizens.Length && currentLevel == _releaseLevels[_currentCitizensState])
        {
            CitizenEnabled?.Invoke(_citizens[_currentCitizensState]);
            IsNewPrisionFree = true;
            _citizens[_currentCitizensState].gameObject.SetActive(true);
            _congratulationWindow.gameObject.SetActive(true);
            
            PlayerPrefs.SetInt(KeySave.FREE_CITIZENS, _currentCitizensState + 1);
        }
    }
}