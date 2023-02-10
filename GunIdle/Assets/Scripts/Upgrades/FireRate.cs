using UnityEngine;

public class FireRate : Upgrade
{
    private void Awake() => ResetValues();

    public override float CalculateValue(float value, float step)
    {
        return value - step;
    }

    public override float GetSaveValue()
    {
        if(PlayerPrefs.HasKey(KeySave.FIRE_RATE))
        {
            return PlayerPrefs.GetFloat(KeySave.FIRE_RATE);
        }

        return 0f;
    }

    public override float GetSavePrice()
    {
        if (PlayerPrefs.HasKey(KeySave.PRICE_FIRE_RATE))
        {
            return PlayerPrefs.GetFloat(KeySave.PRICE_FIRE_RATE);
        }

        return 0f;
    }

    public override int GetSaveLevel()
    {
        if (PlayerPrefs.HasKey(KeySave.LEVEL_FIRE_RATE))
        {
            return PlayerPrefs.GetInt(KeySave.LEVEL_FIRE_RATE);
        }

        return 1;
    }

    public override void SetSaveValue(float value)
    {
        PlayerPrefs.SetFloat(KeySave.FIRE_RATE, value);
    }

    public override void SetSavePrice(float value)
    {
        PlayerPrefs.SetFloat(KeySave.PRICE_FIRE_RATE, value);
    }

    public override void SetSaveLevel(int value)
    {
        PlayerPrefs.SetInt(KeySave.LEVEL_FIRE_RATE, value);
    }

    public override void ResetValues()
    {
        SetStartValue(MaxValue);
        _borderValue = MinValue;
    }
}