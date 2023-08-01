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

    float jumpCnt;

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
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(x, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCnt > 0)
        {
            rb.velocity = new Vector2(x, 0.1f);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Force);

            jumpCnt--;
        }

        if (rb.velocity.y == 0)
            jumpCnt = jumpCount;
    }
}
