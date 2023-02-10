using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailDisplay : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _value;

    private char _dollar = '$';
    private int _delayShowTime = 3;

    private void OnEnable() => _button.onClick.AddListener(RestartLevel);

    private void OnDisable() => _button.onClick.RemoveListener(RestartLevel);

    public void Show() => Invoke(nameof(Enable), _delayShowTime);

    public void SetValue(float reward)
    {
        float value = (float)Math.Round(reward, 1);
        _value.text = value.ToString() + _dollar;
    }

    private void Enable() => gameObject.SetActive(true);

    private void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}