using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Starter : MonoBehaviour, IPointerDownHandler
{
    [Header("Main")]
    [SerializeField] private GameObject _upgradeDisplay;
    [SerializeField] private Level _level;
    [SerializeField] private Level _levelService;

    public event Action levelStarted;
    private bool _isStart;

    public void OnPointerDown(PointerEventData eventData)
    {      
        if(_isStart != true)
        {
            _level.StartGame();
            levelStarted?.Invoke();

            _upgradeDisplay.SetActive(false);
            _isStart = true;
        }    
    }
}