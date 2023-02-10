using System;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] protected Button _back;
    [SerializeField] protected CharacterWallet _wallet;

    public event Action Opened;
    public event Action Closed;

    public void Disable()
    {
        gameObject.SetActive(false);
        Closed?.Invoke();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        Opened?.Invoke();
    }
}