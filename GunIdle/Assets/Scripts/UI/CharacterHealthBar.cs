using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : HealthBar
{
    [SerializeField] private Image _background;
    [SerializeField] private Transform[] _offsets;

    private float _offsetStep = 0.1f;
    private void OnEnable() 
    {
        _character.HealthChanged += OnChangeHealth;
        _character.HPRecalculated += ResizeBar;
    }


    private void OnDisable() 
    {
        _character.HealthChanged -= OnChangeHealth;
        _character.HPRecalculated -= ResizeBar;
    }

    private void Start() => Show();

    private void ResizeBar(int barNumber)
    {
        MoveBar(_background.transform, barNumber);
        MoveBar(_bar.transform, barNumber);
    }

    private void MoveBar(Transform image, int barNumber)
    {
        image.localScale = new Vector3(image.localScale.x + (_offsetStep * barNumber), image.localScale.y, image.localScale.z);
        image.position = _offsets[barNumber].position;
    }
}