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

    public void Gameover()
    {
        gameoverPanel.SetActive(true);
        StartCoroutine(Fade(1));
    }

    void SeenMove() => ScnenManager.instance.MoveMainScene();

    private IEnumerator Fade(int value)
    {
        if (value == 1)
        {
            yield return new WaitForSeconds(waitTime / 10);
        }
        Time.timeScale = 1;

        DOTween.To(() => image.fillAmount, val => image.fillAmount = val, value, fadeTime).OnComplete(() => {
            if (value == 1)
            {
                Invoke("SeenMove", waitTime / 2);
            }
        });
    }
}
