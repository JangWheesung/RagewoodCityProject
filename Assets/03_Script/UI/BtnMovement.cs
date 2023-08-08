using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnMovement : MonoBehaviour
{
    private void Awake()
    {
        Invoke("BtnAppear", 0.5f);
    }

    private void BtnAppear()
    {
        transform.DOMoveX(2700, 1).SetEase(Ease.OutElastic);
    }

    public void BtnDisappear()
    {
        transform.DOMoveX(3300, 1).SetEase(Ease.InBack);
        GameoverManager.instance.FadeIn();
    }
}
