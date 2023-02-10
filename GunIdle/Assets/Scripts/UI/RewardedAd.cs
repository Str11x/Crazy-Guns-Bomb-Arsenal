using Agava.YandexGames;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameAnalyticsSDK;

public class RewardedAd : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _rewardUI;
    [SerializeField] private int _initialReward = 10;
    [SerializeField] private float _rewardMultiplier = 2;
    [SerializeField] private CharacterWallet _wallet;

    private int _digitsCount = 2;
    private string[] _names = { "", "K", "M", "B", "T" };
    private int _amountReductionStart = 1000;
    private int _reward;
    private readonly string _money = "$";
    private readonly string _plus = "+";
    private readonly string _rewardtypeAdClick = "rewardtype-ad-click";
    private readonly string _gold = "gold";
    private readonly string _rewardText = "reward";
    private readonly string _rewardButton = "reward button";
    private int _adStep = 5;
    private Action _adOpened;
    private Action <bool> _interstitialAdClosed;
    private Action _adRewarded;
    private Action _adClosed;
    private Action <string> _adErrorOccured;

    private void Start()
    {
        YandexGamesSdk.Initialize();
        TryShowVideoAD();
        UpdateRewardValue();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        _adOpened += OnAdOpened;
        _adRewarded += OnAdRewarded;
        _adClosed += OnAdClosed;
        _interstitialAdClosed += OnInterstitialAdClosed;
        _adErrorOccured += OnAdErrorOccured;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
        _adOpened -= OnAdOpened;
        _adRewarded -= OnAdRewarded;
        _adClosed -= OnAdClosed;
        _interstitialAdClosed -= OnInterstitialAdClosed;
        _adErrorOccured -= OnAdErrorOccured;
    }

    public void OnClick()
    {
        VideoAd.Show(_adOpened, _adRewarded, _adClosed, _adErrorOccured); 
        GameAnalytics.NewDesignEvent(_rewardtypeAdClick);

#if YANDEX_GAMES && !UNITY_EDITOR
        VideoAd.Show(_adOpened, _adRewarded, _adClosed, _adErrorOccured);        
#endif
#if VK_GAMES
        Agava.VKGames.VideoAd.Show(_adRewarded, _adErrorOccured);
#endif
#if UNITY_EDITOR

#endif

    }

    private void TryShowVideoAD()
    {
        int currentLevel = (PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) + 1);
        int lastAdLevel = PlayerPrefs.GetInt(KeySave.LAST_AD_LEVEL);

        if (currentLevel % _adStep == 0 && lastAdLevel != currentLevel)
        { 
            InterstitialAd.Show(_adOpened, _interstitialAdClosed, _adErrorOccured);
            PlayerPrefs.SetInt(KeySave.LAST_AD_LEVEL, currentLevel);
        }           
    }

    private void UpdateRewardValue()
    {
        _reward = (int)(PlayerPrefs.GetInt(KeySave.ABILITY_PRICE) * _rewardMultiplier);

        if (_reward < _initialReward)
            _reward = _initialReward;

        _rewardUI.text = _plus + RoundReward(_reward);
    }

    private string RoundReward(int money)
    {
        int count = 0;

        while (count + 1 < _names.Length && money >= _amountReductionStart)
        {
            money /= _amountReductionStart;
            count++;
        }

        float value = (float)Math.Round((float)money, _digitsCount);
        string convertValue = value.ToString();
        string convertReward = convertValue + _names[count] + _money;

        return convertReward;
    }

    private void OnAdErrorOccured(string error) => EnableSound();

    private void OnAdClosed() => EnableSound();

    private void OnInterstitialAdClosed(bool wasShow) => EnableSound();

    private void EnableSound()
    {
        AudioListener.pause = false;
        AudioListener.volume = PlayerPrefs.GetFloat(KeySave.AUDIO_LEVEL);
    }

    private void OnAdRewarded()
    {
        UpdateRewardValue();
        _wallet.ApplyMoney((float)_reward);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, _gold, _reward, _rewardText, _rewardButton);
        gameObject.SetActive(false);
    }

    private void OnAdOpened()
    {
        AudioListener.pause = true;
        AudioListener.volume = 0f;
    }
}