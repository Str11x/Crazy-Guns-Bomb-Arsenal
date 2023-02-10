using UnityEngine;
using UnityEngine.UI;

public class CharacterShieldBar : HealthBar
{
    [SerializeField] private Button _shieldButton;
    [SerializeField] private Image _shieldUI;
    [SerializeField] private ParticleSystem _disableEffect;

    private void OnEnable()
    {
        _character.ShieldChanged += OnChangeHealth;
        _character.ShieldBroken += Disable;
        
        if(PlayerPrefs.GetInt(KeySave.ABILITY_SHIELD) == 1)
            _character.ActivateShield();
    }

    private void OnDisable()
    {
        _character.ShieldChanged -= OnChangeHealth;
        _character.ShieldBroken -= Disable;
    }

    private void Start() => Show();

    private void Disable()
    {
        _disableEffect.Play();
        ClearShieldPrefs();
        _shieldUI.gameObject.SetActive(false);
        _shieldButton.gameObject.SetActive(false);
    }

    private void ClearShieldPrefs() => PlayerPrefs.SetInt(KeySave.ABILITY_SHIELD, 0);
}