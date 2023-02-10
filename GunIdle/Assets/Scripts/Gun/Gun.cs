using UnityEngine;
using Cinemachine;

[SelectionBase]
public class Gun : MonoBehaviour
{
    [SerializeField] private FireRate _upgrade;
    [SerializeField] private GunForward _gunForward;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private float _timeBeetwenShoot;
    [SerializeField] private CinemachineVirtualCamera _gunCamera;
    [SerializeField] private GameObject _gunTop;
    [SerializeField] private GameObject _aim;
    [SerializeField] private CharacterStand _characterStand;
    [SerializeField] private float _timeToStopedShootEffect = .1f;
    [SerializeField] private float _shakeOffset = .7f;
    [SerializeField] private BulletSpawner _bulletSpawner;

    private GunMove _gunMove;
    private GunDestroyer _gunDestroyer;
    private GunAnimator _gunAnimator;
    private float _currentTime;
    private bool _isShoot;
    private bool _isDie;
    private float _timeBetwenShootDifficult = 1.2f;
    private CinemachineBasicMultiChannelPerlin _cinemachineVirtualBasic;

    private void OnEnable()
    {
        _upgrade.ValueChanged += OnChangeValue;
        _characterStand.HealthIsOver += Die;
    }

    private void OnDisable()
    {
        _upgrade.ValueChanged -= OnChangeValue;
        _characterStand.HealthIsOver -= Die;
    } 

    private void Start()
    {
        _gunMove = GetComponent<GunMove>();
        _gunDestroyer = GetComponent<GunDestroyer>();
        _gunAnimator = GetComponent<GunAnimator>();
        _cinemachineVirtualBasic = _gunCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _currentTime = _timeBeetwenShoot;
    }

    private void Update()
    {
        if (_isShoot && Input.GetMouseButton(0) && _gunForward.IsMaxHeatReached != true)
        {
            _gunForward.ChangeState(true);
            _currentTime += Time.deltaTime;

            if (_currentTime >= _timeBeetwenShoot)
            {
                Shoot();
                _currentTime = 0;
            }
        }
        else
        {
            _gunForward.ChangeState(false);
            _gunAnimator.Idle();
        }
    }

    public void ChangeState(bool flag)
    {       
        _isShoot = flag;
        _aim.SetActive(flag);
    }

    public void Enable() => gameObject.SetActive(true);

    public void Disable() => gameObject.SetActive(false);

    public bool IsLowHP() => _characterStand.IsLowHPReached;

    public void Die()
    {
        _isDie = true;
        _gunMove.ChangeState();
        _gunAnimator.Deactivate();
        _gunDestroyer.enabled = true;
    }

    private void Shoot()
    {
        _gunAnimator.Shoot();
        _bulletSpawner.FindFreeBullet();
        _shootSound.Play();
        _shootEffect.Play();
        _cinemachineVirtualBasic.m_AmplitudeGain = _shakeOffset;
        Invoke(nameof(StopShootEffect), _timeToStopedShootEffect);
    }

    private void StopShootEffect() => _cinemachineVirtualBasic.m_AmplitudeGain = 0f;

    private void OnChangeValue(float value) => _timeBeetwenShoot = value * _timeBetwenShootDifficult;
}