using GameAnalyticsSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private GunShop _gunShop;
    [SerializeField] private CharacterWallet _wallet;
    [SerializeField] private int _upgradeMinPrice;
    [SerializeField] private int _upgradeMaxPrice;
    [SerializeField] private float _rewardMinValue;
    [SerializeField] private float _rewardMaxValue;
    [SerializeField] private float _fireRateMaxValue;
    [SerializeField] private float _fireRateMinValue;
    [SerializeField] private int _numberInShop;

    [Header("Images")]
    [SerializeField] private Image _backgroundItem;
    [SerializeField] private Image _purchasedState;
    [SerializeField] private Image _purchasedChar;
    [SerializeField] private Image _availableState;
    [SerializeField] private Image _turrel;
    [SerializeField] private Image _blockLabel;
    [SerializeField] private Image _blockBackground;
    [SerializeField] private Image _unselectedChar;

    [Header("Buttons")]
    [SerializeField] private Button _take;
    [SerializeField] private Button _falseBuy;
    [SerializeField] private Button _used;
    [SerializeField] private Button _normalBuyButtonState;

    [Header("Text")]
    [SerializeField] private TMP_Text _availableText;
    [SerializeField] private TMP_Text _purchasedText;
    [SerializeField] private TMP_Text _blockChar;
    [SerializeField] private TMP_Text _price;

    private Color _fullTransparancy = new Color(255, 255, 255, 255);
    private float _cleanPrice;
    private int _purchasedStateNumber = 1;
    private readonly char _dollar = '$';
    private readonly string _purchaseKey = "IsPurchase";
    private readonly string _gold = "gold";
    private readonly string _gunShopText = "gun shop";

    public float FireRateMinValue => _fireRateMinValue;
    public float FireRateMaxValue => _fireRateMaxValue;
    public float RewardMinValue => _rewardMinValue;
    public float RewardMaxValue => _rewardMaxValue;
    public int UpgradeMinPrice => _upgradeMinPrice;
    public int UpgradeMaxPrice => _upgradeMaxPrice;
    public bool IsActive { get; private set; }
    public bool IsPurchased { get; private set; }

    private void Awake()
    {
        float.TryParse(_price.text, out float result);
        _cleanPrice = result;
        _price.text += _dollar;
    }
    public void SetActiveState() => IsActive = true;

    public void SetAvailable()
    {
        ShowWeaponImage();

        _availableState.enabled = true;
        _availableText.enabled = true;
        _price.enabled = true;
    }

    public void SetPurchasedUnactive()
    {
        ShowWeaponImage();

        _purchasedState.enabled = true;
        _purchasedText.enabled = true;
        _take.gameObject.SetActive(true);
        _used.gameObject.SetActive(false);
        _falseBuy.gameObject.SetActive(false);
        _normalBuyButtonState.gameObject.SetActive(false);
        _unselectedChar.enabled = true;
        IsActive = false;
    }

    public void SetPickedUp()
    {
        ShowWeaponImage();
        SetPurchasedState();

        _gunShop.PickedUpWeapon(this);
        _availableState.enabled = false;
        _availableText.enabled = false;
        _purchasedState.enabled = true;
        _purchasedText.enabled = true;
        _used.gameObject.SetActive(true);
        _unselectedChar.enabled = false;
        _purchasedChar.enabled = true;
        _price.enabled = false;
        IsActive = true;
    }

    public void SoundPickedUp() => _gunShop.WeaponPickedupSound();

    public void SoundBuy() => _gunShop.WeaponBuySound();

    public float GetPrice()
    {
        return _cleanPrice;
    }

    public void Buy()
    {
        if (_wallet.Money >= _cleanPrice)
        {
            _wallet.Buy(_cleanPrice);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, _gold, _cleanPrice, _gunShopText, gameObject.name);
        }        
    }

    public void SetAvailableForPurchase()
    {
        _falseBuy.gameObject.SetActive(false);
        _normalBuyButtonState.gameObject.SetActive(true);
        _normalBuyButtonState.interactable = true;
    }

    public void SetUnavailableForPurchase()
    {
        _normalBuyButtonState.gameObject.SetActive(false);
        _falseBuy.gameObject.SetActive(true);
    }

    public void SetPurchasedState() 
    {
        IsPurchased = true;
        PlayerPrefs.SetInt(gameObject.name + _purchaseKey, _purchasedStateNumber);
    }

    private void ShowWeaponImage()
    {
        _backgroundItem.color = _fullTransparancy;
        _turrel.color = _fullTransparancy;
        _blockChar.enabled = false;
        _blockBackground.enabled = false;
        _blockLabel.gameObject.SetActive(false);
    }
}