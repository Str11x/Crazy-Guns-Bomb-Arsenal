using UnityEngine;
using UnityEngine.UI;

public class SoundMuteHandler : MonoBehaviour
{
    [SerializeField] private Sprite _mute;
    [SerializeField] private Sprite _unmute;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    private float _audioOff = 0f;
    private float _audioOn = 0.8f;

    private void OnEnable() => _button.onClick.AddListener(SoundMuteButtonOn);

    private void OnDisable() => _button.onClick.RemoveListener(SoundMuteButtonOn);

    private void Start()
    {
        if (PlayerPrefs.GetFloat(KeySave.AUDIO_LEVEL) == _audioOff)
        {
            AudioListener.pause = true;
            AudioListener.volume = _audioOff;
            _image.sprite = _mute;
        }
        else
        {
            AudioListener.pause = false;
            AudioListener.volume = _audioOn;
            _image.sprite = _unmute;
        }
    }

    public void InitilizeStartAudioLevel() 
    {
        if (PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) == 0)
            PlayerPrefs.SetFloat(KeySave.AUDIO_LEVEL, _audioOn);

        Debug.Log(PlayerPrefs.GetFloat(KeySave.AUDIO_LEVEL));
    }

    public void OnApplicationFocus(bool hasFocus)
    { 
        AudioListener.pause = !hasFocus;
        AudioListener.volume = PlayerPrefs.GetFloat(KeySave.AUDIO_LEVEL);
    }

    private void SoundMuteButtonOn()
    {
        if (PlayerPrefs.GetFloat(KeySave.AUDIO_LEVEL) == _audioOn)
        {
            AudioListener.pause = true;
            _image.sprite = _mute;
            AudioListener.volume = _audioOff;
            PlayerPrefs.SetFloat(KeySave.AUDIO_LEVEL, _audioOff);
        }
        else
        {
            AudioListener.pause = false;
            _image.sprite = _unmute;
            AudioListener.volume = _audioOn;
            PlayerPrefs.SetFloat(KeySave.AUDIO_LEVEL, _audioOn);
        }
    }
}