using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour
{
    public static GaugeManager instance;

    [SerializeField] private Image image;
    [SerializeField] private int[] maxGauge;
    private int gaugeLevel = 0;

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
            RouletteOpen();
        }
    }

    private void RouletteOpen()
    {

    }

    public void RouletteSpin()
    {

    }
}
