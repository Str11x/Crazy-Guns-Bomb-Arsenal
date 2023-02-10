public class FireRate : Upgrade
{
    private void Awake() => SetStartValue(_maxValue);

    public override float CalculateValue(float value, float step)
    {
        return value - step;
    }
}
