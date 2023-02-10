using System;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _value;
    [SerializeField] private CharacterWallet _wallet;

    private int _digitsCount = 2;
    private string _moneyText = "$";
    private string[] _names = { "", "K", "M", "B", "T" };
    private int _amountReductionStart = 1000;

    private void OnEnable() => _wallet.ChangeMoney += OnChangeMoney;

    private void OnDisable() => _wallet.ChangeMoney -= OnChangeMoney;

    private void OnChangeMoney(float money)
    {
        int count = 0;

        while (count + 1 < _names.Length && money >= _amountReductionStart)
        {
            money /= _amountReductionStart;
            count++;
        }

        float value = (float)Math.Round(money, _digitsCount);
        string convertValue = value.ToString();
        _value.text = convertValue + _names[count] + _moneyText;
    }
}