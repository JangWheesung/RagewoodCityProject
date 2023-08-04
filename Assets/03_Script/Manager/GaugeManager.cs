using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GaugeManager : MonoBehaviour
{
    public static GaugeManager instance;

    [SerializeField] private Image image;
    [SerializeField] private RectTransform roulettePanel;
    [SerializeField] private RectTransform rouletteImage;
    [SerializeField] private Button rouletteBtn;
    [SerializeField] private float rouletteTime;
    [SerializeField] private int[] maxGauge;

    private int gaugeLevel = 0;
    private float spinSpeed;

    private void Awake()
    {
        instance = this;
    }

    public void GaugeUp()
    {
        image.fillAmount += (1f / maxGauge[gaugeLevel]);
        if (image.fillAmount >= 1)
        {
            image.fillAmount = 0;
            gaugeLevel = maxGauge.Length - 1 <= gaugeLevel ? gaugeLevel : gaugeLevel + 1;
            rouletteBtn.onClick.AddListener(RouletteSpin);
            RouletteOpen();
        }
    }

    private void Update()
    {
        rouletteImage.Rotate(0, 0, spinSpeed);
    }

    private void RouletteOpen()
    {
        roulettePanel.DOMoveY(550, 1f).SetEase(Ease.OutCirc);
    }

    private void RouletteSpin()
    {
        rouletteBtn.onClick.RemoveListener(RouletteSpin);
        spinSpeed = 10f;
    }

    private IEnumerator stopSpin()
    {
        yield return new WaitForSeconds(rouletteTime);
        //Mathf.Lerp();
    }
}
