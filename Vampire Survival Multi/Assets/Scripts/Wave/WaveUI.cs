using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [Header("UI 오브젝트")]
    [SerializeField] private TextMeshProUGUI tmpUI;
    [SerializeField] private TextMeshProUGUI waveLevelText;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image timerImage;
    [SerializeField] private Sprite normalTimer;
    [SerializeField] private Sprite redTimer;

    // 현재 타이머 상태
    private bool isCloseToTimeOut = false;

    // 참조 데이터
    private WaveData waveData;
    private WaveResource waveResource;

    private void Start()
    {
        waveData = WaveData.Instance;
        waveResource = WaveResource.Instance;
    }

    private void Update()
    {
        float time = waveData.RemainTime;

        UpdateTimer((int)time);
        UpdateSlider(time);
        UpdateLevel(waveData.WaveLevel);
    }

    private void UpdateTimer(int time)
    {
        int min = time / 60;
        int sec = time % 60;

        string timer = min.ToString("00") + ":" + sec.ToString("00");

        tmpUI.text = "<Wave>" +
            "\r\nWave : " + waveData.WaveLevel +
            "\r\nTimer : " + timer +
            "\r\nMob Count : " + waveData.MobCount;
    }

    private void UpdateSlider(float time)
    {
        if (waveData.WaveLevel > 0)
        {
            float waveTime = waveResource.GetWaveTime(waveData.WaveLevel);

            // 남은 시간 슬라이드로 표현
            timerSlider.maxValue = waveTime;
            timerSlider.value = waveTime - time;

            // 남은 시간에 따른 슬라이드바 스프라이트 변경
            if (isCloseToTimeOut == false && time <= 10)
            {
                isCloseToTimeOut = true;

                timerImage.sprite = redTimer;
            }
            else if (isCloseToTimeOut && time > 10)
            {
                isCloseToTimeOut = false;

                timerImage.sprite = normalTimer;
            }
        }
    }

    private void UpdateLevel(int level)
    {
        waveLevelText.text = $"Wave : {level}";
    }
}