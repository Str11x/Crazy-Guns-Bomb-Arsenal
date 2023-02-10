using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Animator))]
public class StartDisplay : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => _animator.enabled = true;
}