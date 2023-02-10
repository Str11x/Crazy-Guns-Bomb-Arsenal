using System.Collections.Generic;
using UnityEngine;

public class UpgradeValuesChanger : MonoBehaviour
{
    [SerializeField] private List<Item> _items;

    public int GetCurrentWeaponMinPrice()
    {
        int minPrice = _items[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)].UpgradeMinPrice;
        return minPrice;
    }

    public int GetCurrentWeaponMaxPrice()
    {
        int maxPrice = _items[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)].UpgradeMaxPrice;
        return maxPrice;
    }

    public float GetCurrentMinReward()
    {
        float minValue = _items[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)].RewardMinValue;
        return minValue;
    }

    public float GetCurrentMaxReward()
    {
        float maxValue = _items[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)].RewardMaxValue;
        return maxValue;
    }

    public float GetCurrentFireRateMinValue()
    {
        float minValue = _items[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)].FireRateMinValue;
        return minValue;
    }

    public float GetCurrentFireRateMaxValue()
    {
        float maxValue = _items[PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER)].FireRateMaxValue;
        return maxValue;
    }
}