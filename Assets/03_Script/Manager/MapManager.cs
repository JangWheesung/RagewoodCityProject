using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform camRange;
    [SerializeField] private Transform ground;

    private void Update()
    {
        StageMove();
    }

    void StageMove()
    {
        if (player.position.x - ground.position.x > 60 || player.position.x - ground.position.x < -60) //¿À¸¥ÂÊ, ¿ÞÂÊ
        {
            ground = Instantiate(ground, new Vector3(player.position.x, ground.position.y), Quaternion.identity, GameObject.Find("Grounds").transform);
            camRange.position = new Vector2(player.position.x, camRange.position.y);
        }
    }
}
