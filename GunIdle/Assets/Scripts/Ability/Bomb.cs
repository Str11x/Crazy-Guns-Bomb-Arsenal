using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] protected ParticleSystem Effect;
    [SerializeField] protected Rigidbody AbilityBody;
    [SerializeField] private AudioSource _explosion;
    [SerializeField] protected EnemySpawner _spawner;

    private int _fallOffset = 1;

    public void Activate()
    {
        Effect.transform.position = AbilityBody.transform.position + new Vector3(_fallOffset, _fallOffset, 0);
        Effect.Play();
        _explosion.Play();
        AbilityBody.gameObject.SetActive(false);
        AffectToEnemies();
    }

    private void AffectToEnemies() => _spawner.DamageAllNearEnemies(this, transform.position);
}