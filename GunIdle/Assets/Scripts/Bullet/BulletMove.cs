using System.Collections;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private MeshRenderer _mesh;

    private WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();

    public void Shoot() => StartCoroutine(ShootMove());

    private IEnumerator ShootMove()
    {
        while (_mesh.enabled == true)
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            yield return _fixedUpdate;
        }
    }
}