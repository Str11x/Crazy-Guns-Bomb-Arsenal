using TMPro;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private TMP_Text _totalValue;

    private CanvasGroup _canvasGroup;

    private void OnEnable() => _level.CountChange += OnCountChange;

    private void OnDisable() => _level.CountChange -= OnCountChange;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    public void Show() => _canvasGroup.alpha = 1f;

    public void Hide() => _canvasGroup.alpha = 0f;

    private void OnCountChange(int value, int totalValue)
    {
        _value.text = value.ToString();
        _totalValue.text = totalValue.ToString();
    }
}