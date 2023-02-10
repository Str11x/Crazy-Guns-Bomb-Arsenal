using UnityEngine;
using UnityEngine.Events;

public class CharacterWallet : MonoBehaviour
{
    private float _money;
    private float _startMoneyCount = 3;

    public float Money => _money;

    public UnityAction<float> ChangeMoney;

    private void Start()
    {
        //PlayerPrefs.SetInt(KeySave.MONEY, 0);
        if (PlayerPrefs.HasKey(KeySave.MONEY))
            _money = PlayerPrefs.GetFloat(KeySave.MONEY);

        if (PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) == 0)
            _money = _startMoneyCount;
        //_money = 100000000;
        ChangeMoney?.Invoke(_money);
    }

    public void ApplyMoney(float count)
    {
        _money += count;
        ChangeMoney?.Invoke(_money);
        PlayerPrefs.SetFloat(KeySave.MONEY, _money);
    }

    public void Buy(float count)
    {
        _money -= count;
        ChangeMoney?.Invoke(_money);
        PlayerPrefs.SetFloat(KeySave.MONEY, _money);
    }
}