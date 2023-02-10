using UnityEngine;

public class TargetCorrector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
            enemy.CorrectPathToTarget();
    }
}