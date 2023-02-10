using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _usedButton;
    [SerializeField] private Button _unavailableButton;
    [SerializeField] private TMP_Text _price;

    private readonly char _money = '$';
    public bool IsBuyed { get; private set; }
    public float Price { get; private set; }

    public void SetPurchased()
    {
        _buyButton.gameObject.SetActive(false);
        _usedButton.gameObject.SetActive(true);
        _unavailableButton.gameObject.SetActive(false);
        _usedButton.enabled = false;
        IsBuyed = true;
    }

    public void SetUnavailable()
    {
        _unavailableButton.gameObject.SetActive(true);
        _unavailableButton.enabled = false;
        _usedButton.gameObject.SetActive(false);
        _buyButton.gameObject.SetActive(false);
    }

    public void SetAvailable()
    {
        if(IsBuyed != true)
        {
            _unavailableButton.gameObject.SetActive(false);
            _usedButton.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(true);
        }       
    }

    public void SetNewPrice(int price)
    {
        Price = price;
        _price.text = price.ToString() + _money;
    }
}