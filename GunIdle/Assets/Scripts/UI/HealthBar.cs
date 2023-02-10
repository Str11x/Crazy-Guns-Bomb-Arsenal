using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    [SerializeField] protected Image _bar;
    [SerializeField] protected CharacterStand _character;
    [SerializeField] protected float _delta = 5f;

    protected Coroutine _fadeOut;
    protected Coroutine _fade;

    private void OnEnable() => _character.StopHealthCalculate += StopCoroutines;

    private void OnDisable() => _character.StopHealthCalculate -= StopCoroutines;

    private void Start() => Hide();

    public void Show()
    {
        _bar.color = new Color(_bar.color.r, _bar.color.g, _bar.color.b, 1f);

        if (_fadeOut != null)
            StopCoroutine(_fadeOut);
    }

    public void Hide() => _bar.color = new Color(_bar.color.r, _bar.color.g, _bar.color.b, 0f);

    protected void OnChangeHealth(int health, int maxHealth)
    {
        if(health <= 0)
        {
            StopCoroutines();
            _bar.fillAmount = 0;
        }

        float value = (float)health / (float)maxHealth;

        Show();

        if(_fade != null)
            StopCoroutine(_fade);

        _fade = StartCoroutine(ChangeValue(value));
    }

    protected IEnumerator ChangeValue(float targetValue)
    {
        while (_bar.fillAmount != targetValue)
        {
            float value = Mathf.MoveTowards(_bar.fillAmount, targetValue, _delta * Time.deltaTime);

            _bar.fillAmount = value;

            yield return null;
        }
    }

    private void StopCoroutines() => StopAllCoroutines();
}