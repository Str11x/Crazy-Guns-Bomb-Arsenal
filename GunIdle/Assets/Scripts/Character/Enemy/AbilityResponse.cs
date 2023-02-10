using System.Collections.Generic;
using UnityEngine;

public class AbilityResponse : MonoBehaviour
{
    [SerializeField] private bool _doesFireBombWork;
    [SerializeField] private bool _doesFreezeBombWork;
    [SerializeField] private bool _isBoss;
    [SerializeField] private List <Bomb> _deadlyAbility = new List<Bomb>();

    public float BossBombPower { get; private set; } = 0.4f;
    public float FreezeBombPower { get; private set; } = 0.3f;
    public float FireBombPower { get; private set; } = 0.75f;
    public float SpeedInFreeze { get; private set; } = 0.5f;

    public bool BombReaction => _doesFireBombWork;
    public bool IsFreezeBombReaction => _doesFreezeBombWork;
    public bool IsBoss => _isBoss;

    public bool IsCurrentAbilityDeadly(Bomb ability)
    {
        foreach(var item in _deadlyAbility)
        {
            if(item.GetType() == ability.GetType())
                return true;
        }

        return false;
    }
}