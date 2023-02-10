using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuChangeState : MonoBehaviour
{
    [SerializeField] private SoundMuteHandler _sound;
    [SerializeField] private Button _leaderBoard;
    [SerializeField] private Starter _starter;
    [SerializeField] private RewardedAd _reward;

    private float _audioOn = 0.8f;
    private bool _isOpen;
    private bool _isShopClosed;

    private void Start() => _sound.InitilizeStartAudioLevel();

    private void OnEnable() => _starter.levelStarted += CloseShopButtons;

    private void OnDisable() => _starter.levelStarted -= CloseShopButtons;

    public void DoChange()
    {
        if(_isOpen != true)
        {
            _sound.gameObject.SetActive(true);
            _leaderBoard.gameObject.SetActive(true);
            _isOpen = true;
        }
        else
        {
            _sound.gameObject.SetActive(false);
            _leaderBoard.gameObject.SetActive(false);
            _isOpen = false;
        }
    }

    private void CloseShopButtons() => _isShopClosed = true;
}