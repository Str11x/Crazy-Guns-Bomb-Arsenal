using UnityEngine;
using UnityEngine.UI;

public class WeaponTools : MonoBehaviour
{
    [SerializeField] private GunShop _gunShop;
    [SerializeField] private AbilityShop _abilityInShop;
    [SerializeField] private Starter _starter;
    [SerializeField] private Button _weaponShopButton;
    [SerializeField] private Button _abilityShopButton;
    [SerializeField] private GameObject _abilitiesInGame;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _rewardedMenuButton;

    private void Start() => ActiveAllButtons();

    private void OnEnable()
    {
        _starter.levelStarted += UnactiveAllButtons;
        _starter.levelStarted += _abilityInShop.ShowIcons;
        _abilityInShop.Opened += UnactiveAllButtons;
        _abilityInShop.Closed += ActiveAllButtons;
        _gunShop.Opened += UnactiveAllButtons;
        _gunShop.Closed += ActiveAllButtons;
        _weaponShopButton.onClick.AddListener(_gunShop.Enable);
        _abilityShopButton.onClick.AddListener(_abilityInShop.Enable);
    }

    private void OnDisable()
    {
        _starter.levelStarted -= UnactiveAllButtons;
        _starter.levelStarted += _abilityInShop.ShowIcons;
        _abilityInShop.Opened -= UnactiveAllButtons;
        _gunShop.Opened -= UnactiveAllButtons;
        _gunShop.Closed -= ActiveAllButtons;
        _abilityInShop.Closed -= ActiveAllButtons;
        _weaponShopButton.onClick.RemoveListener(_gunShop.Enable);
        _abilityShopButton.onClick.RemoveListener(_abilityInShop.Enable);
    }

    public void ActiveAllButtons()
    {
        EnableRewardedMenuButton();
        EnableAbilityShopButton();
        EnableWeaponShopButton();      
        EnableAbilitiesButton();
        EnableMainMenuButton();
    }

    private void UnactiveAllButtons()
    {
        DisableWeaponShopButton();     
        DisableAbilityShopButton();
        DisableMainMenuButton();
        DisableRewardedMenuButton();
    }

    private void DisableRewardedMenuButton() => _rewardedMenuButton.gameObject.SetActive(false);

    private void EnableRewardedMenuButton() => _rewardedMenuButton.gameObject.SetActive(true);

    private void DisableMainMenuButton() => _mainMenuButton.gameObject.SetActive(false);

    private void EnableMainMenuButton() => _mainMenuButton.gameObject.SetActive(true);

    private void DisableAbilitiesButton() => _abilitiesInGame.gameObject.SetActive(false);

    private void EnableAbilitiesButton() => _abilitiesInGame.gameObject.SetActive(true);

    private void DisableWeaponShopButton() => _weaponShopButton.gameObject.SetActive(false);

    private void DisableAbilityShopButton() => _abilityShopButton.gameObject.SetActive(false);

    private void EnableWeaponShopButton() => _weaponShopButton.gameObject.SetActive(true);

    private void EnableAbilityShopButton() => _abilityShopButton.gameObject.SetActive(true);
}