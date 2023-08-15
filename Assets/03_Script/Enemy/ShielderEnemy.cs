using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderEnemy : EnemyFSM
{
    [Header("ShieldObj")]
    [SerializeField] private GameObject shield;
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;

    [Space(10)]
    [SerializeField] private float jumpAmount = 30;

    protected override void Update()
    {
        base.Update();
        shield.transform.position = ShieldTrs().position;
    }

    Transform ShieldTrs()
    {
        //양수면 플레리어가 오른쪽에 있다.
        return playerTrs.transform.position.x - transform.position.x >= 0 ? rightPos : leftPos;
    }

    protected override void AttackState()
    {
        isGas = false;

        if (rb.velocity.y == 0)
            rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
    }
}
