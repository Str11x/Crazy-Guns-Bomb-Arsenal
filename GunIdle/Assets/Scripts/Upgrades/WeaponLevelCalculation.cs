using System;
using UnityEngine;

public class WeaponLevelCalculation : MonoBehaviour
{
    [SerializeField] private Upgrade _stamina;
    [SerializeField] private Upgrade _fireRate;
    [SerializeField] private Upgrade _reward;
    [SerializeField] private GunsStorage _gunStorage;

    private float _weaponLevel;
    private float _abilitiesLevels;
    private float _upgradeThreshold = 0.7f;

    public event Action WeaponUpgraded;
    public event Action LevelUp;

    private void Start() => Reset();

    private void OnEnable()
    {
        _abilitiesLevels = _stamina.StepCount + _fireRate.StepCount + _reward.StepCount;
        _gunStorage.WeaponChanged += Reset;
        _stamina.LevelUp += UpLevel;
        _fireRate.LevelUp += UpLevel;
        _reward.LevelUp += UpLevel;
    }

    private void OnDisable()
    {
        _gunStorage.WeaponChanged -= Reset;
        _stamina.LevelUp -= UpLevel;
        _fireRate.LevelUp -= UpLevel;
        _reward.LevelUp -= UpLevel;
    }

    public void UpLevel()
    {
        _weaponLevel++;
        LevelUp?.Invoke();

        if (IsWeaponUpgrade() && IsNotLastWeapon() && IsMessageAvailable())
        {
            WeaponUpgraded?.Invoke();

            int nextWeapon = PlayerPrefs.GetInt(KeySave.WEAPON_NUMBERS) + 1;
            PlayerPrefs.SetInt(KeySave.WEAPON_NUMBERS, nextWeapon);
        }           
    }

    private bool IsWeaponUpgrade()
    {
        if (_weaponLevel >= _upgradeThreshold * _abilitiesLevels)
            return true;

        return false;
    }

    private void Reset()
    {
        _weaponLevel = _stamina.GetSaveLevel() + _fireRate.GetSaveLevel() + _reward.GetSaveLevel();
    }

    private bool IsNotLastWeapon() 
    {
        return PlayerPrefs.GetInt(KeySave.WEAPON_NUMBERS) + 1 != _gunStorage.GunsCount; 
    }

    private bool IsMessageAvailable()
    {
        int currentmessageNumber = PlayerPrefs.GetInt(KeySave.WEAPON_MESSAGE);
        int weaponNumber = PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER);

        if (currentmessageNumber == weaponNumber)
        {
            PlayerPrefs.SetInt(KeySave.WEAPON_MESSAGE, currentmessageNumber + 1);
            return true;
        }

        return false;
    }
}