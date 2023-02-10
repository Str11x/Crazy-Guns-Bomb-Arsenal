using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UpgradeValuesChanger))]
public class GunsStorage : MonoBehaviour
{
    [Header("Upgrades")]
    [SerializeField] private Upgrade _staminaUI;
    [SerializeField] private Upgrade _fireRateUI;
    [SerializeField] private Upgrade _rewardUI;
    [SerializeField] private Level _levelService;

    private UpgradeValuesChanger _valuesChanger;
    private List<Gun> _guns = new List<Gun>();
    private Gun _currentGun;
    private string _stamina = "Stamina";
    private string _fireRate = "Fire_Rate";
    private string _reward = "Reward";
    private string _staminaPrice = "Stamina_Price";
    private string _fireRatePrice = "Fire_Rate_Price";
    private string _rewardPrice = "Reward_Price";
    private string _staminaValue = "Stamina_Value";
    private string _fireRateValue = "Fire_Rate_Value";
    private string _rewardValue = "Reward_Value";

    public int GunsCount => _guns.Count;

    public event Action WeaponChanged;

    private void Awake()
    {
        _valuesChanger = GetComponent<UpgradeValuesChanger>();
        _guns = GetComponentsInChildren<Gun>(true).ToList();

        _currentGun = _guns[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)];

        foreach (Gun gun in _guns)
        {
            if (gun != _currentGun)
                gun.Disable();
        }

        _levelService.ChangeCurrentGun(_currentGun);
        SetCurrentWeaponPrices();
        SetCurrentWeaponReward();
        SetCurrentFireRateValues();
    }

    public void GiveWeapon()
    {     
        _currentGun = _guns[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)];
        _currentGun.Enable();
        _levelService.ChangeCurrentGun(_currentGun);
        LoadWeaponUpgrades();       
        WeaponChanged?.Invoke();
    }

    public void DisableCurrentGun() => _guns[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)].Disable();

    public void SaveCurrentWeaponUpgrades()
    {
        PlayerPrefs.SetInt(_stamina + _currentGun.name, _staminaUI.GetSaveLevel());
        PlayerPrefs.SetInt(_fireRate + _currentGun.name, _fireRateUI.GetSaveLevel());
        PlayerPrefs.SetInt(_reward + _currentGun.name, _rewardUI.GetSaveLevel());

        PlayerPrefs.SetFloat(_rewardPrice + _currentGun.name, _rewardUI.GetSavePrice());
        PlayerPrefs.SetFloat(_fireRatePrice + _currentGun.name, _fireRateUI.GetSavePrice());
        PlayerPrefs.SetFloat(_staminaPrice + _currentGun.name, _staminaUI.GetSavePrice());

        PlayerPrefs.SetFloat(_staminaValue + _currentGun.name, _staminaUI.GetSaveValue());
        PlayerPrefs.SetFloat(_fireRateValue + _currentGun.name, _fireRateUI.GetSaveValue());
        PlayerPrefs.SetFloat(_rewardValue + _currentGun.name, _rewardUI.GetSaveValue());
    }

    private void LoadWeaponUpgrades()
    {
        SetCurrentWeaponPrices();
        SetCurrentWeaponReward();
        SetCurrentFireRateValues();

        if (PlayerPrefs.HasKey(_stamina + _currentGun.name))
        {
            LoadLevels();
            LoadPrices();
            LoadValues();      
            LoadUpgradesUI();
        }
        else
        {           
            ResetWeaponStats();
            ShowInitialDisplays();
        }       
    }

    private void ShowInitialDisplays()
    {
        _staminaUI.ShowInitialWeaponDisplay();
        _fireRateUI.ShowInitialWeaponDisplay();
        _rewardUI.ShowInitialWeaponDisplay();
    }

    private void LoadPrices()
    {
        float staminaPrice = PlayerPrefs.GetFloat(_staminaPrice + _currentGun.name);
        float fireRatePrice = PlayerPrefs.GetFloat(_fireRatePrice + _currentGun.name);
        float rewardPrice = PlayerPrefs.GetFloat(_rewardPrice + _currentGun.name);

        PlayerPrefs.SetFloat(KeySave.PRICE_STAMINA, staminaPrice);
        PlayerPrefs.SetFloat(KeySave.PRICE_FIRE_RATE, fireRatePrice);
        PlayerPrefs.SetFloat(KeySave.PRICE_REWARD, rewardPrice);
    }

    private void LoadLevels()
    {
        int staminaLevel = PlayerPrefs.GetInt(_stamina + _currentGun.name);
        int fireRateLevel = PlayerPrefs.GetInt(_fireRate + _currentGun.name);
        int rewardLevel = PlayerPrefs.GetInt(_reward + _currentGun.name);

        PlayerPrefs.SetInt(KeySave.LEVEL_STAMINA, staminaLevel);
        PlayerPrefs.SetInt(KeySave.LEVEL_FIRE_RATE, fireRateLevel);
        PlayerPrefs.SetInt(KeySave.LEVEL_REWARD, rewardLevel);
    }

    private void LoadValues()
    {
        float staminaValue = PlayerPrefs.GetFloat(_staminaValue + _currentGun.name);
        float fireRateValue = PlayerPrefs.GetFloat(_fireRateValue + _currentGun.name);
        float rewardValue = PlayerPrefs.GetFloat(_rewardValue + _currentGun.name);

        PlayerPrefs.SetFloat(KeySave.STAMINA, staminaValue);
        PlayerPrefs.SetFloat(KeySave.FIRE_RATE, fireRateValue);
        PlayerPrefs.SetFloat(KeySave.REWARD, rewardValue);
    }

    private void ResetWeaponStats()
    {
        PlayerPrefs.SetInt(KeySave.LEVEL_STAMINA, 0);
        PlayerPrefs.SetInt(KeySave.LEVEL_FIRE_RATE, 0);
        PlayerPrefs.SetInt(KeySave.LEVEL_REWARD, 0);
        PlayerPrefs.SetFloat(KeySave.PRICE_STAMINA, 0);
        PlayerPrefs.SetFloat(KeySave.PRICE_FIRE_RATE, 0);
        PlayerPrefs.SetFloat(KeySave.PRICE_REWARD, 0);
    }

    private void LoadUpgradesUI()
    {
        _staminaUI.ShowLoadedValues();
        _fireRateUI.ShowLoadedValues();
        _rewardUI.ShowLoadedValues();
    }

    private void SetCurrentWeaponPrices()
    {
        int minPrice = _valuesChanger.GetCurrentWeaponMinPrice();
        int maxPrice = _valuesChanger.GetCurrentWeaponMaxPrice();

        _staminaUI.SetNewPrice(minPrice, maxPrice);
        _fireRateUI.SetNewPrice(minPrice, maxPrice);
        _rewardUI.SetNewPrice(minPrice, maxPrice);
    }

    private void SetCurrentWeaponReward()
    {
        float minValue = _valuesChanger.GetCurrentMinReward();
        float maxValue = _valuesChanger.GetCurrentMaxReward();

        _rewardUI.SetNewBordersValues(minValue, maxValue);
    }
    
    private void SetCurrentFireRateValues()
    {
        float minValue = _valuesChanger.GetCurrentFireRateMinValue();
        float maxValue = _valuesChanger.GetCurrentFireRateMaxValue();

        _fireRateUI.SetNewBordersValues(minValue, maxValue);
    }
}