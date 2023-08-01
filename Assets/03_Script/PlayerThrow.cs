using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : PlayerRoot
{
    [SerializeField] private GameObject grenade;
    [SerializeField] private GameObject circle;
    [SerializeField] private float throwSpeed;
    [SerializeField] private float gravity;

    private void Update()
    {
        DirectionControl();
    }

    private void DirectionControl()
    {
        if (Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;

            Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector2 point = Camera.main.ScreenToWorldPoint(mousePos);
            DrawParabola(DirectionSetting(point));
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;

            Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector2 point = Camera.main.ScreenToWorldPoint(mousePos);
            ThrowGrenade(DirectionSetting(point));
        }
    }

    private Vector2 DirectionSetting(Vector3 point)
    {
        return (point - transform.position).normalized;
    }

    private void DrawParabola(Vector2 dir)
    {
        lineRenderer.SetPosition(0, transform.position);

        float time = 0;
        float timeStep = 0.02f; // 시뮬레이션 간격
        Vector2 currentPosition = transform.position;
        Vector2 currentVelocity = dir * throwSpeed;

        int positionCount = 1;
        while (true)
        {
            Vector2 gravityForce = Vector2.down * gravity;
            currentVelocity += gravityForce * timeStep;
            currentPosition += currentVelocity * timeStep;
            time += timeStep;

            // 궤적 그리기
            lineRenderer.positionCount = ++positionCount;
            lineRenderer.SetPosition(positionCount - 1, currentPosition);

            // 땅에 닿았을 때까지 반복
            if (currentPosition.y <= -15)
            {
                break;
            }
        }
    }

    private void ThrowGrenade(Vector2 dir)
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameObject newGrenade = Instantiate(grenade, transform.position, Quaternion.identity);
            newGrenade.GetComponent<Rigidbody2D>().AddForce(dir * throwSpeed, ForceMode2D.Impulse);
        }
    }
}
