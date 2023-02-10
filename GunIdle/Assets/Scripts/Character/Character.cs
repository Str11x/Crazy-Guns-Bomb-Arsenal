using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Rigidbody _hitRigidbody;
    [SerializeField] private List<Rigidbody> _bones;

    private void Start() => ChangeState(true);

    private void ChangeState(bool flag)
    {
        foreach (var bone in _bones)
            bone.isKinematic = flag;
    }

    public void TakeLastHit(float force)
    {
        transform.SetParent(null);
        ChangeState(false);

        _hitRigidbody.AddForce(_hitRigidbody.transform.up * force, ForceMode.Impulse);
    }
}