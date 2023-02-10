using UnityEngine;

public class EffectDisabler : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private ParticleSystem _effect;

    private void OnEnable() => _level.LevelFailed += Disable;

    private void OnDisable() => _level.LevelFailed -= Disable;

    private void Disable() => _effect.gameObject.SetActive(false);
}