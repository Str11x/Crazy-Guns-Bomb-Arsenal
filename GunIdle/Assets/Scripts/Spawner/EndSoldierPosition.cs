using UnityEngine;

public class EndSoldierPosition : MonoBehaviour
{
    public Vector3 Position => transform.position;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SoldierMove soldier) && soldier.IsShootReached)
            soldier.Die();
    }
}