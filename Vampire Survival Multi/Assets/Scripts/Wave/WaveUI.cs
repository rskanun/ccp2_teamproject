using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [Header("UI 오브젝트")]
    [SerializeField] private TextMeshProUGUI tmpUI;
    [SerializeField] private Slider timerSlider;  // Slider 컴포넌트를 위한 변수 추가

    // 임시 참조 데이터
    private WaveData waveData;

    private void Start()
    {
        waveData = WaveData.Instance;
        timerSlider.maxValue = waveData.InitialTime;  // 초기 최대 시간 설정
    }

    private void Update()
    {
        int time = (int)waveData.RemainTime;

        UpdateTimer(time);
    }

    private void UpdateTimer(int time)
    {
        int min = time / 60;
        int sec = time % 60;

        string timer = min.ToString("00") + ":" + sec.ToString("00");

        tmpUI.text = "Mob Count : " + waveData.MobCount;
    }
    private void UpdateSlider(int time)
    {
        timerSlider.value = waveData.InitialTime - time;  // 슬라이더 값 업데이트
    }
}