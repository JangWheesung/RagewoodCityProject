using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScore;
    float value;
    Ease ease;

    private void Awake()
    {
        Spin(20f);
        Bound();
        bestScore.text = $"BEST SCORE : {PlayerPrefs.GetInt("BestScore")}";
    }

    void Spin(float value)
    {
        transform.DORotate(new Vector3(0, 0, value), 1f).SetEase(Ease.OutCubic).OnComplete(() => {
            transform.DORotate(Vector3.zero, 1f).SetEase(Ease.InCubic).OnComplete(() => { Spin(-value); });
        });
    }

    void Bound()
    {
        value = value == 50f ? 60f : 50f;
        ease = ease == Ease.InBounce ? Ease.OutBounce : Ease.InBounce;
        DOTween.To(() => bestScore.fontSize, val => bestScore.fontSize = val, value, 0.2f).SetEase(ease).OnComplete(() => {
            Bound();
        });
    }
}
