using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStorage : MonoBehaviour
{
    [SerializeField] private List<Ability> _abilities;
    [SerializeField] private Starter _starter;
    [SerializeField] private Level _level;
    [SerializeField] private CharacterStand _character;

    [SerializeField] private AudioSource _click;
    [SerializeField] private AudioSource _bombNoise;
    [SerializeField] private AudioSource _bombWhistle;

    private bool[] _chargeAbilities;
    private int _bomb = 0;
    private int _true = 1;
    private int _false = 0;
    private int _freeze = 1;
    private bool _isFightActive;

    public event Action AbilityUsed;
    public event Action SavedValuesUpdated;

    private void Start()
    {
        _chargeAbilities = new bool[_abilities.Count];
        UpdateCharge();
    } 

    private void OnEnable()
    {
        _level.Started += EnableFightState;
        _level.LevelCompleted += DisableFightState;
        _level.LevelFailed += DisableFightState;
        _character.ShieldBroken += DeleteShield;
    } 

    private void OnDisable()
    {
        _level.Started -= EnableFightState;
        _level.LevelCompleted -= DisableFightState;
        _level.LevelFailed -= DisableFightState;
        _character.ShieldBroken -= DeleteShield;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && _chargeAbilities[_bomb] == true && _isFightActive)
            Use(_bomb);

        if (Input.GetKey(KeyCode.W) && _chargeAbilities[_freeze] == true && _isFightActive)
            Use(_freeze);
    }

    public void Use(int number)
    {
        _abilities[number].gameObject.SetActive(true);
        DeleteAbility(number);
        AbilityUsed?.Invoke();
        PlayAudio(number);
    }

    public int GetBombSavedValue()
    {
        return PlayerPrefs.GetInt(KeySave.ABILITY_BOMB);
    }

    public int GetFreezeSavedValue()
    {
        return PlayerPrefs.GetInt(KeySave.ABILITY_FREEZE);
    }

    public int GetShieldSavedValue()
    {
        return PlayerPrefs.GetInt(KeySave.ABILITY_SHIELD);
    }

    public void SavePurchase(int number)
    {
        if (number == _bomb)
            PlayerPrefs.SetInt(KeySave.ABILITY_BOMB, _true);
        else if (number == _freeze)
            PlayerPrefs.SetInt(KeySave.ABILITY_FREEZE, _true);
        else
            PlayerPrefs.SetInt(KeySave.ABILITY_SHIELD, _true);

        _chargeAbilities[number] = true;
    }

    private void EnableFightState() => _isFightActive = true;

    private void DeleteAbility(int number)
    {
        if (number == _bomb)
        {
            DeleteBomb();
            _chargeAbilities[_bomb] = false;
        }
        else if (number == _freeze)
        {
            DeleteFreeze();
            _chargeAbilities[_freeze] = false;
        }
        
        SavedValuesUpdated?.Invoke();
    }

    private void DisableFightState() => _isFightActive = false;

    private void PlayAudio(int abilityNumber)
    {
        _click.Play();

        if (abilityNumber == _bomb)
        {
            _bombNoise.Play();
            _bombWhistle.Play();
        }
        else
        {
            _bombWhistle.Play();
        }
    }

    private void DeleteShield() => PlayerPrefs.SetInt(KeySave.ABILITY_SHIELD, _false);

    private void DeleteBomb() => PlayerPrefs.SetInt(KeySave.ABILITY_BOMB, _false);

    private void DeleteFreeze() => PlayerPrefs.SetInt(KeySave.ABILITY_FREEZE, _false);

    private void UpdateCharge()
    {
        if (GetBombSavedValue() == _true)
            _chargeAbilities[_bomb] = true;
        if (GetFreezeSavedValue() == _true)
            _chargeAbilities[_freeze] = true;       
        if (GetShieldSavedValue() == _true)
            _chargeAbilities[_freeze] = true;
    }
}