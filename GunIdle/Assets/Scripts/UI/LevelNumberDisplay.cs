using UnityEngine;
using TMPro;

public class LevelNumberDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _levelNumber;
    [SerializeField] private Starter _starter;
    [SerializeField] private CitizensPrison _citizensPrison;

    private void OnEnable()
    {    
        _starter.levelStarted += Show;
        _citizensPrison.CitizenEnabled += Hide;
    }

    private void OnDisable()
    {
        _starter.levelStarted -= Show;
        _citizensPrison.CitizenEnabled -= Hide;
    }

    private void Hide(Citizen citizen) => gameObject.SetActive(false);
    private void Show()
    {
        _name.gameObject.SetActive(true);
        _levelNumber.gameObject.SetActive(true);
        _levelNumber.text = (PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) + 1).ToString();
    }
}