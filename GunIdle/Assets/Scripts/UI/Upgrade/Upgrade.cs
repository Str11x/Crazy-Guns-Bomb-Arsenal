using GameAnalyticsSDK;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Upgrade : MonoBehaviour
{
    [SerializeField] private CharacterWallet _wallet;
    [SerializeField] private TMP_Text _priceValue;
    [SerializeField] private TMP_Text _levelValue;
    [SerializeField] private Button _button;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _minValue;
    [SerializeField] private int _stepCount;
    [SerializeField] private int _maxPrice;
    [SerializeField] private int _minPrice;
    [SerializeField] private Image _backGround;
    [SerializeField] private TMP_Text _maximum;

    private int _maxLevel;
    private int _minLevel = 0;
    private Color _noActive = new Color(.5f, .5f, .5f, 1f);
    private Color _active = new Color(1f, 1f, 1f, 1f);
    private float _stepValue;
    private int _stepPrice;
    private int _stepLevel;
    private float _price;
    private int _level;
    private float _value;
    private float _currentMoney;
    protected float _borderValue;
    private char _dollar = '$';
    private readonly string _gold = "gold";
    private readonly string _upgrade = "upgrade";

    public float MaxValue => _maxValue;
    public float MinValue => _minValue;
    public float StepCount => _stepCount;
    public float Price => _price;


    public UnityAction<float> ValueChanged;

    public UnityAction LevelUp;

    private void OnEnable()
    {
        _price = _minPrice;
        _level = _minLevel;
        _button.onClick.AddListener(SetValue);
        _wallet.ChangeMoney += OnChangeMoney;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SetValue);
        _wallet.ChangeMoney -= OnChangeMoney;
    }

    private void Start()
    {
        _maxLevel = _stepCount;
        _stepValue = (_maxValue - _minValue) / _stepCount;
        _stepPrice = (_maxPrice - _minPrice) / _stepCount;
        _stepLevel = (_maxLevel - _minLevel) / _stepCount;

        float value = GetSaveValue();
        float price = GetSavePrice();
        int level = GetSaveLevel();
        _level = level;
        _price = price;

        if (_value != value && value > 0)
        {
            _value = value;
            ValueChanged?.Invoke(_value);
            SetStartDataDisplay();
            return;
        }
 
        SetDataDisplay();
    }

    public void SetNewBordersValues(float minValue, float maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _borderValue = _maxValue;
    }

    public void SetNewPrice(int minPrice, int maxPrice)
    {
        _minPrice = minPrice;
        _maxPrice = maxPrice;
        _stepPrice = (_maxPrice - _minPrice) / _stepCount;
    }

    public void SetValue()
    {
        _wallet.Buy(_price);
        _value = Mathf.Clamp(CalculateValue(_value, _stepValue), _minValue, _maxValue);
        _level = Mathf.Clamp(_level + _stepLevel, _minLevel, _maxLevel);
        _price = Mathf.Clamp(_price + _stepPrice, _minPrice, _maxPrice);

        SetDataDisplay();

        SetSaveValue(_value);
        ValueChanged?.Invoke(_value);
        LevelUp?.Invoke();

        if (_level == _maxLevel)
            SetState(false, _noActive, true);

        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, _gold, _price, _upgrade, gameObject.name);
    }

    public void ShowInitialWeaponDisplay()
    {
        ResetValues();

        _level = GetSaveLevel();
        _price = GetSavePrice();

        _level = Mathf.Clamp(_level + _stepLevel, _minLevel, _maxLevel);
        _price = Mathf.Clamp(_price + _stepPrice, _minPrice, _maxPrice);

        _levelValue.text = _level.ToString();
        _priceValue.text = _price.ToString() + _dollar;

        SetSaveLevel(_level);
        SetSavePrice(_price);
        SetSaveValue(_value);
        ValueChanged?.Invoke(_value);

        UpdateStates();
    }

    public void ShowLoadedValues()
    {
        _value = GetSaveValue();
        ValueChanged?.Invoke(_value);
        _level = GetSaveLevel();
        _price = GetSavePrice();

        _levelValue.text = GetSaveLevel().ToString();
        _priceValue.text = GetSavePrice().ToString() + _dollar;

        UpdateStates();    
    }

    private void SetDataDisplay()
    {
        if(_price == 0)
            _price = Mathf.Clamp(_price + _stepPrice, _minPrice, _maxPrice);

        _levelValue.text = _level.ToString();
        _priceValue.text = _price.ToString() + _dollar;

        SetSaveLevel(_level);
        SetSavePrice(_price);
        CheckMoney(_currentMoney);
    }

    private void SetStartDataDisplay()
    {
        _levelValue.text = _level.ToString();
        _priceValue.text = _price.ToString() + _dollar;

        if (_level == _maxLevel)
        {
            SetState(false, _noActive, true);
        }

        CheckMoney(_currentMoney);
    }

    public void SetStartValue(float value)
    {
        if (GetSaveValue() == 0)
            SetSaveValue(value);

        _value = value;
    }

    private void OnChangeMoney(float money)
    {
        _currentMoney = money;
        CheckMoney(_currentMoney);
    }

    private void CheckMoney(float money)
    {
        float valuePrice = (float)Math.Round(_price, 1);
        float valueMoney = (float)Math.Round(money, 1);

        if (money >= _price && _level != _maxLevel)
            SetState(true, _active, false);
        else
            SetState(false, _active, false);
    }

    private void SetState(bool flag, Color color, bool isMax)
    {
        _backGround.color = color;
        _priceValue.text = _price.ToString() + _dollar;
        _button.interactable = flag;

        if (_level == _maxLevel)
            _priceValue.text = _maximum.text;
    }

    private void UpdateStates()
    {
        if (_level == _maxLevel)
            SetState(false, _noActive, true);
        else
            SetState(true, _active, false);

        CheckMoney(_wallet.Money);
;    }

    public abstract float CalculateValue(float value, float step);

    public abstract float GetSaveValue();

    public abstract float GetSavePrice();

    public abstract int GetSaveLevel();

    public abstract void SetSaveValue(float value);

    public abstract void SetSavePrice(float value);

    public abstract void SetSaveLevel(int value);

    public abstract void ResetValues();
}