using UnityEngine;

public class Reward : Upgrade
{
    private void Awake() => ResetValues();

    public override float CalculateValue(float value, float step)
    {
        return value + step;
    }

    public override float GetSaveValue()
    {
        if (PlayerPrefs.HasKey(KeySave.REWARD))
        {
            return PlayerPrefs.GetFloat(KeySave.REWARD);
        }

        return 0;
    }

    public override float GetSavePrice()
    {
        if (PlayerPrefs.HasKey(KeySave.PRICE_REWARD))
        {
            return PlayerPrefs.GetFloat(KeySave.PRICE_REWARD);
        }

        return 0;
    }

    public override int GetSaveLevel()
    {
        if (PlayerPrefs.HasKey(KeySave.LEVEL_REWARD))
        {
            return PlayerPrefs.GetInt(KeySave.LEVEL_REWARD);
        }

        return 1;
    }

    public override void SetSaveValue(float value)
    {
        PlayerPrefs.SetFloat(KeySave.REWARD, value);
    }

    public override void SetSavePrice(float value)
    {
        PlayerPrefs.SetFloat(KeySave.PRICE_REWARD, value);
    }

    public override void SetSaveLevel(int value)
    {
        PlayerPrefs.SetInt(KeySave.LEVEL_REWARD, value);
    }

    public override void ResetValues() => SetStartValue(MinValue);
}