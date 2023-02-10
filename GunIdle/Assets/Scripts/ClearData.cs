using UnityEngine;

public class ClearData : MonoBehaviour
{
    public void Clear() => PlayerPrefs.DeleteAll();
}