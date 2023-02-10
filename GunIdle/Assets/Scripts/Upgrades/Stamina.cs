using UnityEngine;

public class Stamina : Upgrade
{
    private void Awake() => ResetValues();

    public override float CalculateValue(float value, float step)
    {
        return value + step;
    }

    public override float GetSaveValue()
    {
        if (PlayerPrefs.HasKey(KeySave.STAMINA))
        {
            return PlayerPrefs.GetFloat(KeySave.STAMINA);
        }

        return 0f;
    }

    public override float GetSavePrice()
    {
        if (PlayerPrefs.HasKey(KeySave.PRICE_STAMINA))
        {
            return PlayerPrefs.GetFloat(KeySave.PRICE_STAMINA);
        }

        return 0;
    }

    public override int GetSaveLevel()
    {
        if (PlayerPrefs.HasKey(KeySave.LEVEL_STAMINA))
        {
            return PlayerPrefs.GetInt(KeySave.LEVEL_STAMINA);
        }

        return 1;
    }

    public override void SetSaveValue(float value)
    {
        PlayerPrefs.SetFloat(KeySave.STAMINA, value);
    }

    public override void SetSavePrice(float value)
    {
        PlayerPrefs.SetFloat(KeySave.PRICE_STAMINA, value);
    }

    public override void SetSaveLevel(int value)
    {
        PlayerPrefs.SetInt(KeySave.LEVEL_STAMINA, value);
    }

    public override void ResetValues()
    {
        SetStartValue(MinValue);
        _borderValue = MaxValue;
    }
}