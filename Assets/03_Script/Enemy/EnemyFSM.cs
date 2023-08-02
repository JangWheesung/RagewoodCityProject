using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State { Idle, Attack, Stun }

public class EnemyFSM : PlayerRoot
{
    State state = State.Idle;

    [SerializeField] private float playeRadius;
    public float moveSpeed;
    public float rotateSpeed;
    public float attackPower;
    public float attackDelay;

    private Transform playerTrs;
    private PlayerHP playerHP;

    private GameObject gun;
    private GameObject muzzle;
    private SpriteRenderer gunSp;

    public bool isFaint;
    bool isGas = false;
    bool isAttack = false;

    private void OnEnable()
    {
        state = State.Idle;

        playerTrs = GameObject.FindWithTag("Player").transform;
        playerHP = playerTrs.GetComponent<PlayerHP>();

        gun = transform.GetChild(0).gameObject;
        muzzle = gun.transform.GetChild(0).gameObject;
        gunSp = gun.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Fsm();
    }

    void Fsm()
    {
        if (isFaint)
            state = State.Stun;
        else if (Physics2D.OverlapCircle(transform.position, playeRadius, LayerMask.GetMask("Player")))
            state = State.Attack;
        else
            state = State.Idle;

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
        }
    }

    void IdleState()
    {
        isGas = false;
        isAttack = false;
        lineRenderer.enabled = false;

        float distance = Mathf.Clamp(playerTrs.position.x - transform.position.x, -1f, 1f);
        rb.velocity = new Vector2(distance * moveSpeed, rb.velocity.y);
    }

    void AttackState()
    {
        isGas = false;

        Vector2 direction = new Vector2 (
            gun.transform.position.x - playerTrs.position.x,
            gun.transform.position.y - playerTrs.position.y
        );

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 180f, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(gun.transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);

        gun.transform.rotation = rotation;
        gunSp.flipY = playerTrs.position.x - transform.position.x > 0 ? false : true;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, muzzle.transform.position);
        lineRenderer.SetPosition(1, playerTrs.position);

        if (isAttack) return;
        else isAttack = true;

        StartCoroutine(AttackDelay());
    }

    void StunState()
    {
        spriteRenderer.color = Color.yellow;

        if (isGas) return;
        else isGas = true;
        isAttack = false;
        lineRenderer.enabled = false;

        StartCoroutine(InvokeDelay(() => { isFaint = false; }, 5f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playeRadius);
    }

    IEnumerator AttackDelay()
    {
        Debug.Log("ÇÑ¹ø¸¸");
        while (state == State.Attack)
        {
            playerHP.OnDamage(attackPower);

            lineRenderer.enabled = true;
            yield return new WaitForSeconds(attackDelay);
            lineRenderer.enabled = false;
            yield return new WaitForSeconds(attackDelay);
        }
    }

    IEnumerator InvokeDelay(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
