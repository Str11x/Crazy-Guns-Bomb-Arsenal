using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpikesMovement : MonoBehaviour
{
    private Animator _animator;
    private List <int> _animationList = new List<int>();
    private int _randomAnimation;

    private int _iceAnimation1 = Animator.StringToHash("Ice1");
    private int _iceAnimation2 = Animator.StringToHash("Ice2");
    private int _iceAnimation3 = Animator.StringToHash("Ice3");
    private int _iceAnimation4 = Animator.StringToHash("Ice4");
    private int _iceAnimation5 = Animator.StringToHash("Ice5");

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _animationList.Add(_iceAnimation1);
        _animationList.Add(_iceAnimation2);
        _animationList.Add(_iceAnimation3);
        _animationList.Add(_iceAnimation4);
        _animationList.Add(_iceAnimation5);

        _randomAnimation = Random.Range(0, _animationList.Count);     
        _animator.SetBool(_animationList[_randomAnimation], true);
    }
}