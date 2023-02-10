using UnityEngine;

public class BackGroundTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _background;

    private void Awake()
    {
        if (Input.deviceOrientation != DeviceOrientation.Portrait)
            _background.SetActive(true);
    }
}