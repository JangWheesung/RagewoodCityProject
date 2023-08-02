using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Idle, Attack, Stun, Die }

public class EnemyFSM : MonoBehaviour
{
    State state = State.Idle;

    [SerializeField] private float playeRadius;

    void EnemyAction()
    {
        switch (state)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Stun:
                StunState();
                break;
            case State.Die:
                DieSrate();
                break;
        }
    }

    void IdleState()
    {
        if (Physics2D.OverlapCircle(transform.position, playeRadius, LayerMask.GetMask("Player")))
        {
            state = State.Attack;
        }
        
    }

    void AttackState()
    {

    }

    void StunState()
    {

    }

    void DieSrate()
    {

    }
}
