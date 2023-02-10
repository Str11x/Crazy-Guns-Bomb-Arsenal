using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet[] _bulletPrefab;
    [SerializeField] private Sleeve[] _sleevesPrefab;
    [SerializeField] private Transform _sleevePoint;
    [SerializeField] private Transform[] _shootPoints;
    [SerializeField] private GunsStorage _gunStorage;
    [SerializeField] private int _poolSize = 20;

    private List<Bullet> _bullets = new List<Bullet>();
    private List<Sleeve> _sleeves = new List<Sleeve>();
    private int _currentBullet;
    private Transform _currentShootPoint;
    private int _lastActiveBullet;
    private int _currentActiveBullet;

    public void FindFreeBullet()
    {
        for(int i = _lastActiveBullet; i < _poolSize - 1; i++)
        {
            if(_bullets[i].IsActive != true)
            {
                _lastActiveBullet = i;
                _currentActiveBullet = i;
                break;
            }
            else
            {
                if (i == _poolSize - 2) 
                    i = 0;
                    
                continue;
            }
        }

        SetBulletToShootPoint();
    }
    private void OnEnable() => _gunStorage.WeaponChanged += ChangeBulletsPool;

    private void OnDisable() => _gunStorage.WeaponChanged -= ChangeBulletsPool;

    private void SetBulletToShootPoint()
    {
        _bullets[_currentActiveBullet].transform.position = _currentShootPoint.position;
        _bullets[_currentActiveBullet].transform.rotation = _currentShootPoint.rotation;
        _bullets[_currentActiveBullet].Enable();

        _sleeves[_currentActiveBullet].transform.position = _sleevePoint.position;
        _sleeves[_currentActiveBullet].Enable();
    }

    private void ChangeBulletsPool()
    {
        if(_bullets.Count == _poolSize)
            DeleteOldPool();       

        _currentBullet = PlayerPrefs.GetInt(KeySave.WEAPON_CURRENT_NUMBER);
        _currentShootPoint = _shootPoints[_currentBullet];

        for (int i = 0; i < _poolSize; i++)
        {
            _bullets.Add(Instantiate(_bulletPrefab[_currentBullet], _currentShootPoint.transform.position, _currentShootPoint.rotation));
            _sleeves.Add(Instantiate(_sleevesPrefab[_currentBullet], _sleevePoint.transform.position, _sleevePoint.rotation));
        }
    }

    private void DeleteOldPool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            Destroy(_bullets[i].gameObject);
            Destroy(_sleeves[i].gameObject);
        }

        _bullets.Clear();
        _sleeves.Clear();
    }
}