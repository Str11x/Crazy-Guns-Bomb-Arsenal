using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class NewWeaponDisplay : MonoBehaviour
{
    [SerializeField] Level _levelService;
    [SerializeField] Button _later;
    [SerializeField] Button _buyNewWeapon;
    [SerializeField] GunsStorage _gunsStorage;
    [SerializeField] AudioSource _open;

    private CanvasGroup _canvas;

    private void Awake() => _canvas = GetComponent<CanvasGroup>();

    private void OnEnable()
    {
        _open.Play();
        _later.onClick.AddListener(CloseDisplay);
        _buyNewWeapon.onClick.AddListener(OpenGunShop);
    }

    private void OnDisable()
    {
        _later.onClick.RemoveListener(CloseDisplay);
        _buyNewWeapon.onClick.RemoveListener(OpenGunShop);
    }

    public void Enable() => gameObject.SetActive(true);

    private void CloseDisplay() => gameObject.SetActive(false);

    private void OpenGunShop()
    {
        CloseDisplay();
        _levelService.ShowGunShop();
    }
}