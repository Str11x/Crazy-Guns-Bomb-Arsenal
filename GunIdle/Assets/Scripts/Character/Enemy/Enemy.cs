using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMove))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(EnemyHitbox))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private RewardDisplay _rewardDisplay;
    [SerializeField] private float _timeToHide;
    [SerializeField] private float _hitForce;
    [SerializeField] private Rigidbody _objectToHit;
    [SerializeField] private Rigidbody _head;
    [SerializeField] private Material _dieMaterial;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private ParticleSystem _damageEffect;
    [SerializeField] private Animator _freezeEffect;
    [SerializeField] private int _damage;

    [Header("ExplosiveReaction")]
    [SerializeField] private int _explosiveRange = 1;
    [SerializeField] private int _tossForce = 2;
    [SerializeField] private int _knockbackForce = 2;
    [SerializeField] private int _rightDistanceForce = -1;
    [SerializeField] private int _leftDistanceForce = 5;

    private EnemyAnimator _enemyAnimator;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private EnemyMove _enemyMove;
    private CharacterStand _characterStand;
    private EnemyHitbox _enemyHitbox;
    private readonly string _dieMeshRenderer = "_ColorDim";
    private bool _isBoss;
    private float _bossReward;
    private int _health;
    private int _maxHealth = 100;

    public Vector3 EndPosition { get; private set; }

    public UnityAction<Enemy> Died;
    public UnityAction DamageApplay;
    public UnityAction<bool> StateChanged;
    public UnityAction<int, int> HealthChanged;
    public Color _color;

    private void Awake()
    {
        _enemyMove = GetComponent<EnemyMove>();
        _enemyAnimator = GetComponent<EnemyAnimator>();
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _enemyHitbox = GetComponent<EnemyHitbox>();
    }

    private void Start() => _characterStand.HealthIsOver += _enemyAnimator.Victory;

    private void OnDisable() => _characterStand.HealthIsOver -= _enemyAnimator.Victory;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterStand _stand))
        {
            StateChanged?.Invoke(false);
            _enemyAnimator.Punch();
        }          
    }

    public void SetStand(CharacterStand target) => _characterStand = target;

    public void SetEndPosition(Vector3 position) => EndPosition = position;

    public void CorrectPathToTarget() => _enemyMove.SetNewTarget(_characterStand.Position);

    public void ChangeState(bool flag)
    {
        StateChanged?.Invoke(flag);

        if (!flag)
            _enemyAnimator.Victory();
    }

    public void PlayDamageEffects()
    {
        DamageApplay?.Invoke();

        if (_damageEffect != null)
            _damageEffect.Play();

        if (_rewardDisplay != null)
            _rewardDisplay.Show();

        if (_freezeEffect != null && _enemyHitbox.IsFreezing == true)
        {
            _freezeEffect.gameObject.SetActive(true);
            _enemyAnimator.UseFreezeSpeed();
        }
    }

    public void SetHelth(int helth)
    {
        _health = helth;
        _maxHealth = helth;
        _enemyHitbox.SetHealth(_health, _maxHealth);
        HealthChanged?.Invoke(_health, _maxHealth);
    }

    public void SetSpeed(float speed)
    {
        _enemyHitbox.SetCurrentBossSpeed(speed);
        _enemyMove.SetSpeed(speed);
    }

    public void SetReward(float reward)
    {
        if (_rewardDisplay != null)
            _rewardDisplay.SetReward(reward);
    }

    public void TakeDamage() => _characterStand.ApplyDamage(_damage);

    public void EnableHitSound() => _characterStand.SoundHit();

    public void MakeInvisible() => _skinnedMeshRenderer.enabled = false;

    public void Die()
    {
        if (_isBoss)
            SetReward(_bossReward);

        StateChanged(false);
        _capsuleCollider.enabled = false;
        _head.isKinematic = false;

        ChangeMeshRenderer();
        SimulateDieRigidbody();

        Died?.Invoke(this);
        Invoke(nameof(Hide), _timeToHide);
    }

    private void ChangeMeshRenderer()
    {
        if (_skinnedMeshRenderer.materials.Length > 1)
        {
            _skinnedMeshRenderer.materials[1].color = _dieMaterial.color;
            _skinnedMeshRenderer.materials[1].SetColor(_dieMeshRenderer, _dieMaterial.color);
        }
        else
            _skinnedMeshRenderer.material = _dieMaterial;
    }

    private void SimulateDieRigidbody()
    {
        Vector3 direction = new Vector3 (Random.Range(_rightDistanceForce, _leftDistanceForce), _tossForce, -_knockbackForce);

        _rigidbody.isKinematic = false;
        _enemyAnimator.Disable();

        if (_enemyHitbox.IsExplosiveForce == true)
            _objectToHit.AddForce(((-transform.forward + direction) * _explosiveRange) * _hitForce, ForceMode.Impulse);
        else
            _objectToHit.AddForce(-_objectToHit.transform.forward * _hitForce, ForceMode.Impulse);
    }

    private void Hide() => gameObject.SetActive(false);
}