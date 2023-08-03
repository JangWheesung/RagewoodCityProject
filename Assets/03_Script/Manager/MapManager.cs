using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform camRange;
    [SerializeField] private Transform ground;
    [SerializeField] private Transform backGround;

    private void Update()
    {
        StageMove();
    }

    void StageMove()
    {
        if (NeedInstantiate(80))
        {
            float moveX = Mathf.Clamp(player.position.x - ground.position.x, -1f, 1f) * 153f;
            backGround = Instantiate(backGround, new Vector2(backGround.position.x + moveX, backGround.position.y), Quaternion.identity, GameObject.Find("Grounds").transform);
            ground = Instantiate(ground, new Vector3(ground.position.x + moveX, ground.position.y), Quaternion.identity, GameObject.Find("Grounds").transform);
            camRange.position = new Vector2(player.position.x, camRange.position.y);
        }
    }

    bool NeedInstantiate(float value)
    {
        //¿À¸¥ÂÊ, ¿ÞÂÊs
        return player.position.x - ground.position.x > value || player.position.x - ground.position.x < -value;
    }
}
