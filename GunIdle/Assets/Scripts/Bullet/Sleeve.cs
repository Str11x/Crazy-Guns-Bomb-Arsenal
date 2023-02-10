using UnityEngine;

public class Sleeve : MonoBehaviour
{
    [SerializeField] private float _maxForce;
    [SerializeField] private float _minForce;
    [SerializeField] private float _lifeTime;
    [SerializeField] private Rigidbody _body;
    [SerializeField] private MeshRenderer _mesh;

    private Rigidbody _rigidbody;

    public void Enable()
    {
        _mesh.enabled = true;
        _body.AddForce(transform.forward * Random.Range(_minForce, _maxForce), ForceMode.Impulse);

        Invoke(nameof(Hide), _lifeTime);
    }
    private void Hide() => _mesh.enabled = false;
}