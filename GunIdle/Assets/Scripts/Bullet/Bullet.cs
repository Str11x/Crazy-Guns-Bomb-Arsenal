using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _timeToDisable;
    [SerializeField] private ParticleSystem _destroyEffect;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private BulletMove _bulletMove;
    [SerializeField] private int _damage;
    [SerializeField] private Collider _collider;
    [SerializeField] private bool _isEnemyBullet;

    private float _yOffsetPosition = 1.5f;
    private float _zOffsetPosition = 1;

    public bool IsActive { get; private set; }

    public void Enable()
    {
        gameObject.SetActive(true);
        IsActive = true;
        _mesh.enabled = true;
        _collider.enabled = true;
        _bulletMove.Shoot();
        Invoke(nameof(Disable), _timeToDisable);
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHitbox enemy) && _isEnemyBullet != true)
        {
            Vector3 currentPoint = other.transform.position;

            _destroyEffect.transform.position = new Vector3(currentPoint.x, currentPoint.y + _yOffsetPosition, currentPoint.z + _zOffsetPosition); 
            enemy.ApplyDamage(_damage);
        }
        else if (other.TryGetComponent(out CharacterStand player))
        {
            _destroyEffect.transform.position = other.transform.position;
            player.ApplyDamage(_damage);           
        }

        _destroyEffect.Play();
        Disable();
    }

    public void Disable()
    {
        _mesh.enabled = false;
        _collider.enabled = false;
        Invoke(nameof(DisableGameobject), _timeToDisable);
    }

    private void DisableGameobject()
    {    
         IsActive = false;
         gameObject.SetActive(false);
    }
}