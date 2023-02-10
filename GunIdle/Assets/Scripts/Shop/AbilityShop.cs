using GameAnalyticsSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityShop : Shop
{
    [SerializeField] private AbilityStorage _abilityStorage;
    [SerializeField] private Upgrade _stamina;
    [SerializeField] private Upgrade _fireRate;
    [SerializeField] private Upgrade _reward;
    [SerializeField] private List<Button> _icons;
    [SerializeField] private Canvas _shop;
    [SerializeField] private Button _abilityMenuButton;
    [SerializeField] private Starter _starter;
    [SerializeField] private Level _level;
    [SerializeField] private List<AbilityUI> _abilities;

    private int _bomb = 0;
    private int _freeze = 1;
    private int _shield = 2;
    private int[] _savedValues;
    private int _upgradeCount = 3;
    private float _priceReduce = 0.6f;
    private readonly string _gold = "gold";
    private readonly string _abilityShop = "ability shop";

    private void OnEnable() 
    {
        _abilityStorage.SavedValuesUpdated += FillAbilitiesSavedValues;
        _abilityStorage.AbilityUsed += SetButtonsState;
        _starter.levelStarted += SetButtonsState;
        _level.LevelFailed += HideButtons;
        UpdateShop();
    }

    private void OnDestroy()
    {
        _abilityStorage.SavedValuesUpdated -= FillAbilitiesSavedValues;
        _abilityStorage.AbilityUsed -= SetButtonsState;
        _level.LevelFailed -= HideButtons;
        _starter.levelStarted -= SetButtonsState;
    }

    public void InitialUI()
    {       
        _savedValues = new int[_abilities.Count];
        FillAbilitiesSavedValues();
        ChangeAbilitiesStateUI();
        UpdateShop();
    }

    public void UseAbility(int number) => _abilityStorage.Use(number);

    public void SetAbility(int number) 
    {
        _abilityStorage.SavePurchase(number);
        _wallet.Buy(_abilities[number].Price);
        FillAbilitiesSavedValues();
        ChangeAbilitiesStateUI();
        UpdateShop();
        
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, _gold, _abilities[number].Price, _abilityShop, _abilities[number].name);
    }

    private void SetButtonsState()
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            if (_savedValues[i] != 0)
                _icons[i].interactable = true;
            else
                _icons[i].interactable = false;
        }
    }

    private void HideButtons()
    {
        for (int i = 0; i < _abilities.Count; i++)
            _icons[i].gameObject.SetActive(false);
    }

    private void ChangeAbilitiesStateUI()
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            if (_savedValues[i] != 0)
                _abilities[i].SetPurchased();
        }
    }

    private void FillAbilitiesSavedValues()
    {
        _savedValues[_bomb] = _abilityStorage.GetBombSavedValue();
        _savedValues[_freeze] = _abilityStorage.GetFreezeSavedValue();
        _savedValues[_shield] = _abilityStorage.GetShieldSavedValue();
    }

    private void UpdateShop()
    {
        UpdatePrice();
        CheckMoney();
    }

    public void ShowIcons()
    {
        if(PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) != 0)
        {
            _icons[_bomb].gameObject.SetActive(true);
            _icons[_bomb].interactable = false;

            _icons[_freeze].gameObject.SetActive(true);
            _icons[_freeze].interactable = false;

            if (_savedValues[_shield] != 0)
                _icons[_shield].gameObject.SetActive(true);
        }
    }

    private void UpdatePrice()
    {
        int currentAveragePrice = PlayerPrefs.GetInt(KeySave.ABILITY_PRICE);
        int averagePrice = (int)((_stamina.Price + _fireRate.Price + _reward.Price) / _upgradeCount * _priceReduce);

        if(currentAveragePrice <= averagePrice)
        {
            foreach (AbilityUI ability in _abilities)
                ability.SetNewPrice(averagePrice);

            PlayerPrefs.SetInt(KeySave.ABILITY_PRICE, averagePrice);       
        }
        else
        {
            foreach (AbilityUI ability in _abilities)
                ability.SetNewPrice(currentAveragePrice);
        }
    }

    private void CheckMoney()
    {
        foreach(AbilityUI ability in _abilities)
        {
            if(ability.IsBuyed != true)
            {
                if (_wallet.Money < ability.Price)
                    ability.SetUnavailable();
                else
                    ability.SetAvailable();
            }                   
        }
    }
}