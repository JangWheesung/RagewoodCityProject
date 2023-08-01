using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private GameObject grenade;
    [SerializeField] private float throwSpeed;

    private void Update()
    {
        DirectionControl();
    }

    private void DirectionControl()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector2 point = Camera.main.ScreenToWorldPoint(mousePos);
            ThrowGrenade(DirectionSetting(point));
        }
    }

    private Vector2 DirectionSetting(Vector3 point)
    {
        return (point - transform.position).normalized;
    }

    private void ThrowGrenade(Vector2 dir)
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameObject newGrenade = Instantiate(grenade, transform.position, Quaternion.identity);
            newGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x * throwSpeed, dir.y * throwSpeed), ForceMode2D.Impulse);
        }
    }
}
