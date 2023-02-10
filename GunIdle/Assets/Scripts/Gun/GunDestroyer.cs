using UnityEngine;

public class GunDestroyer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosions;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxForceDostroy = 1f;
    [SerializeField] private float _minForceDostroy = -1f;
    [SerializeField] private Rigidbody _forwardGun;
    [SerializeField] private Rigidbody _topGun;
    [SerializeField] private Character _character;
    [SerializeField] private GunAnimator _gunAnimator;
    [SerializeField] private Projector _shadow;
    [SerializeField] private Rigidbody _bakedWeaponPart;

    private void Start()
    {
        _gunAnimator.Deactivate();
        _shadow.enabled= false;

        _bakedWeaponPart.GetComponent<Collider>().enabled= true;
        _forwardGun.isKinematic = false;
        _topGun.isKinematic = false;
        _bakedWeaponPart.isKinematic = false;

        Vector3 directionForwardGun = new Vector3(Random.Range(_minForceDostroy, _maxForceDostroy), Random.Range(_minForceDostroy, _maxForceDostroy), Random.Range(_minForceDostroy, _maxForceDostroy));
        Vector3 directionTopGun = new Vector3(Random.Range(_minForceDostroy, _maxForceDostroy), Random.Range(_minForceDostroy, _maxForceDostroy), Random.Range(_minForceDostroy, _maxForceDostroy));

        _forwardGun.AddForce(directionTopGun * CalculateForce(), ForceMode.Impulse);
        _topGun.AddForce(directionForwardGun * CalculateForce(), ForceMode.Impulse);

        _explosions.Play();

        _character.TakeLastHit(CalculateForce());

        _bakedWeaponPart.transform.parent = null;
        _bakedWeaponPart.AddForce(directionTopGun * CalculateForce(), ForceMode.Impulse);
    }

    private float CalculateForce()
    {
        return Random.Range(_minForce, _maxForce);
    }
}