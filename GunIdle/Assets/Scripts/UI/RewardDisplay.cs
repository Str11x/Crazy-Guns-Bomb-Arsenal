using UnityEngine;
using TMPro;

public class RewardDisplay : MonoBehaviour
{
    [SerializeField] private float _transitionTime;
    [SerializeField] private float _duration;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private Animator _animator;

    private int _reward = Animator.StringToHash("Reward");
    private void Start() => transform.LookAt(Camera.main.transform);

    public void Show()
    {
        _value.enabled = true;
        _animator.Play(_reward, -1, _transitionTime);
        Invoke(nameof(Hide), _duration);
    }

    public void SetReward(float reward) => _value.text = "$" + reward.ToString();

    private void Hide() => _value.enabled = false;
}