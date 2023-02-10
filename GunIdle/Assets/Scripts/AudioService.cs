using UnityEngine;

public class AudioService : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Level _level;
    [SerializeField] private GunShop _gunShop;
    [SerializeField] private CharacterStand _character;

    [Header("AudioSources")]
    [SerializeField] private AudioSource _enemyDamage;
    [SerializeField] private AudioSource _reload;
    [SerializeField] private AudioSource _buy;
    [SerializeField] private AudioSource _health;
    [SerializeField] private AudioSource _shield;
    [SerializeField] private AudioSource _shieldCrash;
    [SerializeField] private AudioSource _characterCrash;
    [SerializeField] private AudioSource _characterPain;
    [SerializeField] private AudioSource _startTheme;
    [SerializeField] private AudioSource _gameTheme;
    [SerializeField] private AudioSource _winTheme;
    [SerializeField] private AudioSource _failTheme;
    [SerializeField] private AudioSource [] _enemydDeathVoices;

    private int _randomVoice;
    private int _silent;

    private void OnEnable()
    {
        _level.Started += PlayGameTheme;
        _level.EnemyDie += PlayDieEnemy;
        _level.LevelCompleted += PlayWinSound;
        _level.LevelFailed += PlayFailSound;
        _character.HealthIsOver += PlayCrushGun;
        _character.ShieldBroken += CrashShield;
        _character.ShieldChanged += PlayHitShield;
        _character.HealthChanged += PlayHitHealth;
        _level.DamageDone += PlayDamage;
        _gunShop.WeaponPickedup += PlayReload;
        _gunShop.WeaponBought += PlayBuy;
    }
    private void OnDisable()
    {
        _level.Started -= PlayGameTheme;
        _level.EnemyDie -= PlayDieEnemy;
        _level.LevelCompleted -= PlayWinSound;
        _level.LevelFailed -= PlayFailSound;
        _character.HealthIsOver -= PlayCrushGun;
        _character.ShieldBroken -= CrashShield;
        _character.ShieldChanged -= PlayHitShield;
        _character.HealthChanged -= PlayHitHealth;
        _level.DamageDone -= PlayDamage;
        _gunShop.WeaponPickedup -= PlayReload;
        _gunShop.WeaponBought -= PlayBuy;
    }

    private void Start()
    {
        _startTheme.Play();
    }

    private void PlayWinSound()
    {
        _gameTheme.Stop();
        _winTheme.Play();
    }

    private void PlayFailSound()
    {

        _gameTheme.Stop();
        _failTheme.Play();
    }

    private void PlayDieEnemy()
    {
        _silent = _enemydDeathVoices.Length;
        _randomVoice = Random.Range(0, _silent);

        if (_randomVoice != _silent)          
            _enemydDeathVoices[_randomVoice].Play();
    }
    private void PlayCrushGun() => _characterCrash.Play();

    private void PlayHitShield(int min, int max) => _shield.Play();

    private void PlayHitHealth(int min, int max)
    {
        _characterPain.Play();
        _health.Play();
    }

    private void PlayGameTheme()
    {
        _startTheme.Stop();
        _gameTheme.Play(); 
    }
    private void CrashShield() => _shieldCrash.Play();

    private void PlayBuy() => _buy.Play();

    private void PlayDamage() => _enemyDamage.Play();

    private void PlayReload() => _reload.Play();
}