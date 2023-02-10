using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunShop : Shop
{
    [SerializeField] private GunsStorage _storage;

    private List<Item> _items = new List<Item>();
    private Item _currentActiveItem;
    private readonly string _purchaseKey = "IsPurchase";

    public event Action WeaponPickedup;
    public event Action WeaponBought;

    private void Awake()
    {
        _items = GetComponentsInChildren<Item>(true).ToList();
        _items[0].SetPurchasedState();
    }

    private void OnEnable()
    {
        _back.onClick.AddListener(Disable);
        ShowAvailablePrice();
        ShowWeaponsState();
    }

    private void OnDisable() => _back.onClick.RemoveListener(Disable);

    public void PickedUpWeapon(Item item)
    {
        if (_currentActiveItem != null)
            _currentActiveItem.SetPurchasedUnactive();

        _currentActiveItem = item;
        
        SaveNewPickedUpWeapon(item);
    }

    public void WeaponPickedupSound() => WeaponPickedup?.Invoke();

    public void WeaponBuySound() => WeaponBought?.Invoke();

    private void ShowWeaponsState()
    {
        int _currentWeapon = PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER);

        if(_items[_currentWeapon].IsActive == false)
           _items[_currentWeapon].SetPickedUp();

        _currentActiveItem = _items[_currentWeapon];

        int availableWeaponCount = PlayerPrefs.GetInt(KeySave.WEAPON_NUMBERS);

        for (int i = 0; i <= availableWeaponCount; i++)
        {
            if (_currentWeapon == i)
                continue;

            if (PlayerPrefs.GetInt(_items[i].name + _purchaseKey) == 1)
                _items[i].SetPurchasedUnactive();
            else
                _items[i].SetAvailable();
        }           
    }

    private void ShowAvailablePrice()
    {
        foreach(Item item in _items)
        {
            if (_wallet.Money >= item.GetPrice())
                item.SetAvailableForPurchase();
            else
                item.SetUnavailableForPurchase();
        }
    }

    private void SaveNewPickedUpWeapon(Item item)
    {
        _storage.SaveCurrentWeaponUpgrades();
        _storage.DisableCurrentGun();

        int currentWeaponNumber = _items.IndexOf(item);
        PlayerPrefs.SetInt(KeySave.WEAPON_CURRENT_NUMBER, currentWeaponNumber);
        _storage.GiveWeapon();
    }
}