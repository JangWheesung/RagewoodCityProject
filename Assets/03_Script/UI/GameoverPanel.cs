using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameoverPanel : MonoBehaviour
{
    private RectTransform gameoverText;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        gameoverText = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        image.DOFade(1, 1f).OnComplete(() => {
            Time.timeScale = 1;
            gameoverText.DOMoveY(-200, 2f).SetEase(Ease.OutBack).OnComplete(() => {
                ScnenManager.instance.MoveMainScene();
            });
        });
    }
}
