using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float changeX;

    private void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= changeX)
        {
            transform.position = new Vector3(-changeX, 6.5f, 0);
        }
    }
}
