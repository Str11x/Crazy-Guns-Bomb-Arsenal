using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private Level _level;
    [SerializeField] private Enemy _soldier;
    [SerializeField] private CharacterStand _target;   
    [SerializeField] private EndSoldierPosition _endSoldierPosition;
    [SerializeField] private Starter _starter;
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private List<Enemy> _bosses;
    [SerializeField] private int _endGameBossSpeed = 4;

    [Header("Upgrades")]
    [SerializeField] private Upgrade _stamina;
    [SerializeField] private Upgrade _fireRate;
    [SerializeField] private Upgrade _reward;

    private EnemyHealthCalculation _healthService = new EnemyHealthCalculation();
    private EnemiesCountGenerator _countService = new EnemiesCountGenerator();
    private List<EnemyHitbox> _enemiesBody = new List<EnemyHitbox>();
    private WaitForSeconds _waitTime = new WaitForSeconds(0.025f);
    private LevelData _data;
    private Enemy _enemy;
    private Enemy _newEnemy;
    private Enemy _boss;
    private int _currentLevelNumber;
    private int _enemiesCount;
    private int _soldiersCount;
    private int _newEnemiesCount;
    private float _minOffsetZ = 5;
    private float _maxOffsetZ = 20;
    private float _bossSpeed;
    private int _bossCount;
    private int _enemyHealth;
    private int _bossHealth;
    private float _border = 4;
    private int _easyLevelsNumber = 100;
    private float _bombRadius = 18;
    private readonly string _scenesData = "ScenesData/";
    private readonly string _levelData = "LevelData";

    private void Start()
    {
        _currentLevelNumber = PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) + 1;
 
        ChangeHPValue();

        if (_currentLevelNumber > _easyLevelsNumber)
        {
            CreateRandomEnemies();
        }           
        else
        {
            GetLevelData();
            CreatePreparedEnemies();
        }         
    }

    public void GetLevelData()
    {     
        _data = Resources.Load<LevelData>(_scenesData + _levelData + _currentLevelNumber.ToString());

        _enemy = _data.GetEnemy();
        _newEnemy = _data.GetNewEnemy();
        _boss = _data.GetBoss();
        _enemiesCount = _data.GetEnemiesCount();
        _soldiersCount = _data.GetSoldiersCount();
        _newEnemiesCount = _data.GetNewEnemiesCount();
        _minOffsetZ = _data.GetMinOffsetZ();
        _maxOffsetZ = _data.GetMaxOffsetZ();
        _bossSpeed = _data.GetBossSpeed();
        _bossCount = _data.GetBossCount();
    }

    public void ChangeHPValue()
    {
        _enemyHealth = _healthService.GetValue(false);
        _bossHealth = _healthService.GetBossValue(false);
    }

    public void DamageAllNearEnemies(Bomb bomb, Vector3 explosivePosition) => StartCoroutine(StartFindEnemies(bomb, explosivePosition));

    private void CreateStandartEnemy(Enemy prefab, int count)
    {
        Enemy enemy;
        Vector3 spawnPoint = transform.position;

        for (int i = 0; i < count; i++)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            spawnPoint = new Vector3(transform.position.x + Random.Range(-_border, _border), transform.position.y, spawnPoint.z + Random.Range(-_minOffsetZ, -_maxOffsetZ));
            enemy = Instantiate(prefab, spawnPoint, transform.rotation, transform);
            enemy.SetStand(_target);
            enemy.SetEndPosition(_endSoldierPosition.Position);
             
            if(prefab.TryGetComponent(out Golem golem))
                enemy.SetHelth(_healthService.GetValue(true));
            else
                enemy.SetHelth(_enemyHealth);

            _level.AddEnemy(enemy);
            _enemiesBody.Add(enemy.GetComponent<EnemyHitbox>());
        }
    }

    private void CreateBoss(int count)
    {
        Enemy enemy;
        Vector3 spawnPoint = transform.position;

        for (int i = 0; i < count; i++)
        {
            enemy = Instantiate(_boss, spawnPoint, transform.rotation, transform);
            spawnPoint = new Vector3(transform.position.x + Random.Range(-_border, _border), transform.position.y, spawnPoint.z + Random.Range(-_minOffsetZ, -_maxOffsetZ));
            
            if (_boss.TryGetComponent(out Golem golem))
                enemy.SetHelth(_healthService.GetBossValue(true));
            else
                enemy.SetHelth(_bossHealth);

            enemy.SetSpeed(_bossSpeed);
            enemy.SetStand(_target);

            _level.AddEnemy(enemy);
            _enemiesBody.Add(enemy.GetComponent<EnemyHitbox>());
        }          
    }

    private void CreatePreparedEnemies()
    {
        CreateStandartEnemy(_soldier, _soldiersCount);
        CreateStandartEnemy(_enemy, _enemiesCount);
        CreateStandartEnemy(_newEnemy, _newEnemiesCount);
        CreateBoss(_bossCount);
    }

    private void CreateRandomEnemies()
    {
        _enemiesCount = 0;
        _bossCount = 0;
        int generatedCount;

        foreach (Enemy enemy in _enemies)
        {
            generatedCount = _countService.GetEnemiesCount();
            _enemiesCount += generatedCount;
            CreateStandartEnemy(enemy, generatedCount);
        }

        _boss = _bosses[Random.Range(0, _bosses.Count)];
        generatedCount = _countService.GetBossCount();
        _bossCount += generatedCount;
        _bossSpeed = _endGameBossSpeed;
        CreateBoss(generatedCount);
    }

    private IEnumerator StartFindEnemies(Bomb bomb, Vector3 explosivePosition)
    {
        bool isFreeze = false;

        if (bomb is Freeze)
            isFreeze = true;

        for (int i = 0; i < _enemiesBody.Count; i++)
        {
            float dist = (_enemiesBody[i].transform.position - explosivePosition).magnitude;

            if (dist < _bombRadius)
            {
                if(isFreeze != true)
                    _enemiesBody[i].ApplyDamage(bomb);
                else
                    _enemiesBody[i].ApplyDamage(bomb as Freeze);
            } 

            yield return _waitTime;
        }
    }
}