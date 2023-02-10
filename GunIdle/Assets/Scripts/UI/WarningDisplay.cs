using UnityEngine;

public class WarningDisplay : MonoBehaviour
{
    [SerializeField] private CitizensPrison _citizensPrison;

    private void OnEnable() => _citizensPrison.CitizenEnabled += Hide;

    private void OnDisable() => _citizensPrison.CitizenEnabled -= Hide;

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
    public void Hide(Citizen citizen) => gameObject.SetActive(false);
}