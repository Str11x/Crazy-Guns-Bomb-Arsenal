using UnityEngine;

public class SoldierMove : EnemyMove
{
    public bool IsShootReached { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SoldierPosition shootPosition))
        {
            RotateToTarget();
            StopCoroutine(MovementCoroutine);
            EnemyAnimator.Shoot();
            IsShootReached = true;
        }
    }

    public void BackToSpawn()
    {
        _targetPath = Enemy.EndPosition;
        EnemyAnimator.Run();
        IsTargetReached = false;
        MovementCoroutine = StartCoroutine(Movement());
    }

    public void Die()
    {
        Enemy.Die();
        Enemy.MakeInvisible();
    }
}