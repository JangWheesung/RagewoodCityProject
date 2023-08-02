using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticLifeTime : MonoBehaviour
{
    [SerializeField] private float time;

    private void OnEnable()
    {
        Invoke("PushParticle", time);
    }

    void PushParticle()
    {
        PoolingManager.instance.Push(gameObject);
    }
}
