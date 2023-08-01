using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected new BoxCollider2D collider;
    protected Rigidbody2D rb;
    protected LineRenderer lineRenderer;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }
}
