using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameName : MonoBehaviour
{
    private void Awake()
    {
        Spin(20f);
    }

    void Spin(float value)
    {
        transform.DORotate(new Vector3(0, 0, value), 1f).SetEase(Ease.OutCubic).OnComplete(() => {
            transform.DORotate(Vector3.zero, 1f).SetEase(Ease.InCubic).OnComplete(() => { Spin(-value); });
        });
    }
}
