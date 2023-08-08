using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviour
{
    public static GameoverManager instance;

    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private Image image;
    [SerializeField] private float waitTime;
    [SerializeField] private float fadeTime;

    private void Awake()
    {
        instance = this;

        StartCoroutine(Fade(0));
    }

    public void FadeIn() => StartCoroutine(Fade(1, "GameMove"));

    public void Gameover()
    {
        gameoverPanel.SetActive(true);
        StartCoroutine(Fade(1, "SeenMove"));
    }

    void SeenMove() => ScnenManager.instance.MoveMainScene();
    void GameMove() => ScnenManager.instance.MoveGameScene();

    private IEnumerator Fade(int value, string seenName = null)
    {
        if (value == 1)
        {
            image.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime / 10);
        }
        Time.timeScale = 1;

        DOTween.To(() => image.fillAmount, val => image.fillAmount = val, value, fadeTime).OnComplete(() => {
            if (value == 1)
            {
                Invoke(seenName, waitTime / 2);
            }
            image.gameObject.SetActive(value == 1 ? true : false);
        });
    }
}
