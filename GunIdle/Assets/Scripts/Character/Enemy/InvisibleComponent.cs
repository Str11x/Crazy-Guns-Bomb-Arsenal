using System.Collections;
using UnityEngine;

public class InvisibleComponent : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _mesh;
    [SerializeField] private Projector _shadow;
    [SerializeField] private ParticleSystem _effect;

    WaitForSeconds _invisibleTime = new WaitForSeconds(2);

    private void Start()
    {
        StartCoroutine(MakeInvisible());
    }

    private IEnumerator MakeInvisible()
    {
        while (true)
        {
            if(_mesh.enabled == true)
            {
                _mesh.enabled = false;
                _shadow.enabled = false;
            }
            else
            {
                _shadow.enabled = true;
                _mesh.enabled = true;
            }

            _effect.Play();

            yield return _invisibleTime;
        }
    }
}