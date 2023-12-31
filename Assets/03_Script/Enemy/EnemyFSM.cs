using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Idle, Attack, Stun }

public class EnemyFSM : PlayerRoot
{
    protected State state = State.Idle;

    [SerializeField] protected float playeRadius;
    [SerializeField] public float moveSpeed;
    [SerializeField] protected float rotateSpeed;
    [HideInInspector] public float attackPower;
    [SerializeField] protected float attackDelay;

    protected Transform playerTrs;
    protected PlayerHP playerHP;

    protected GameObject gun;
    protected GameObject muzzle;
    protected SpriteRenderer gunSp;
    protected AudioSource fireSound;

    public bool isFaint;
    protected bool isGas = false;
    protected bool isAttack = false;

    private void OnEnable()
    {
        state = State.Idle;

        playerTrs = GameObject.FindWithTag("Player").transform;
        playerHP = playerTrs.GetComponent<PlayerHP>();

        gun = transform.GetChild(0).gameObject;
        muzzle = gun.transform.GetChild(0).gameObject;
        gunSp = gun.GetComponent<SpriteRenderer>();
        fireSound = GetComponent<AudioSource>();

        moveSpeed = 7;
    }

    protected virtual void Update()
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

    protected virtual void IdleState()
    {
        isGas = false;
        isAttack = false;
        lineRenderer.enabled = false;
        fireSound.Stop();

        float distance = playerTrs.gameObject.activeSelf == true ? Mathf.Clamp(playerTrs.position.x - transform.position.x, -1f, 1f) : 0;
        rb.velocity = new Vector2(distance * moveSpeed, rb.velocity.y);

        if (rb.velocity.y == 0)
            rb.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
    }

    protected virtual void AttackState()
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

        lineRenderer.SetPosition(0, muzzle.transform.position);
        lineRenderer.SetPosition(1, playerTrs.position);

        if (isAttack) return;
        else isAttack = true;

        StartCoroutine(AttackDelay());
    }

    void StunState()
    {
        if (isGas) return;
        else isGas = true;

        isAttack = false;
        lineRenderer.enabled = false;
        fireSound.Stop();

        StartCoroutine(InvokeDelay(() => { isFaint = false; }, 5f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playeRadius);
    }

    protected virtual IEnumerator AttackDelay()
    {
        fireSound.Play();
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
