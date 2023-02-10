using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WonDisplay : MonoBehaviour
{
    [SerializeField] private CitizensPrison _citizensPrison;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private int _displayDelay = 3;
    [SerializeField] private int _prisionerDelay = 6;

    private int _handcraftedLevels = 50;
    private readonly string _true = "true";
    private readonly char _dollar = '$';

    private void OnEnable() => _button.onClick.AddListener(SetNextLevel);

    private void OnDisable() => _button.onClick.RemoveListener(SetNextLevel);

    public void Show()
    {
        if(_citizensPrison.IsNewPrisionFree == true) 
            Invoke(nameof(Enable), _prisionerDelay);
        else
            Invoke(nameof(Enable), _displayDelay);
    }

    public void SetValue(float reward)
    {
        float value = (float)Math.Round(reward, 1);
        _value.text = value.ToString() + _dollar;
    }

    private void Enable() => gameObject.SetActive(true);

    private void SetNextLevel()
    {
        int levelNumber = PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER);

        if (levelNumber + 1 !=_handcraftedLevels)
        {
            PlayerPrefs.SetInt(KeySave.LEVEL, levelNumber);

            levelNumber++;
            PlayerPrefs.SetInt(KeySave.LEVEL_NUMBER, levelNumber);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            PlayerPrefs.SetString(KeySave.LEVEL_TOTAL_COMPLETED, _true);
            System.Random random = new System.Random();

            levelNumber++;
            PlayerPrefs.SetInt(KeySave.LEVEL_NUMBER, levelNumber);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}