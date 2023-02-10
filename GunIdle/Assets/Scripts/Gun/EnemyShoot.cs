using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _shootPoint;

    public void Shoot()
    {
        Bullet bullet = Instantiate(_bullet, _shootPoint.transform.position, _shootPoint.rotation);
        bullet.Enable();
    }
}