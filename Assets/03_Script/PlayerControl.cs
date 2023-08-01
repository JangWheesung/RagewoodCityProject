using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControl : PlayerRoot
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpCount;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;

    float jumpCnt;
    float dashLength = 0;
    bool dashDelay;

    private void Start()
    {
        jumpCnt = jumpCount;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        //양 옆 이동
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(x + dashLength, rb.velocity.y);

        //점프
        if (Input.GetKeyDown(KeyCode.Space) && jumpCnt > 0)
        {
            rb.velocity = new Vector2(x, 0.1f);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

            jumpCnt--;
        }

        if (rb.velocity.y == 0)
            jumpCnt = jumpCount;

        //대시
        if (Input.GetKeyDown(KeyCode.LeftShift) && x != 0 && !dashDelay)
        {
            dashLength = Input.GetAxis("Horizontal") * dashSpeed;
            StartCoroutine(DashDelay(dashTime));
        }
    }

    private IEnumerator DashDelay(float time)
    {
        yield return new WaitForSeconds(0.1f);
        dashLength = 0;
        dashDelay = true;
        yield return new WaitForSeconds(time);
        dashDelay = false;
    }
}
