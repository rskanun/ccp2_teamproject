using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private WaveUI waveUI;

    // 참조 데이터
    private WaveData waveData;

    private bool isGameComplete = false;

    private void Awake()
    {
        waveData = WaveData.Instance;
    }

    private void Start()
    {
        // 게임 데이터 초기화
        InitGameData();

        // 장비 초기 셋팅
        PlayerEquip.Instance.InitEquips();
    }

    private void InitGameData()
    {
        // 게임을 플레이 할 플레이어 목록 가져오기
        List<PlayerData> playerDatas = new List<PlayerData>();

        playerDatas.Add(LocalPlayerData.Instance);

        GameData.Instance.InitData(playerDatas);

        // 웨이브 데이터 초기화
        waveData.InitData();
    }

    /***************************************************************
    * [ 웨이브 진행 ]
    * 
    * 웨이브 진행에 따른 게임 진행 설정
    ***************************************************************/

    private void Update()
    {
        if (isGameComplete == false)
        {
            // 남은 시간이 없거나 모든 몬스터를 제거했을 경우
            if (waveData.RemainTime <= 0 || waveData.MobCount <= 0)
            {
                // 다음 웨이브 진행
                waveData.NextWave();

                // 만약 최종 웨이브 다음으로 넘어갈 경우
                if (waveData.WaveLevel <= 0)
                {
                    // 게임 종료
                    OnComplete();
                }
            }
            else
                waveData.RemainTime -= Time.deltaTime;

            // 타이터 UI 업데이터
            waveUI.UpdateTimer((int)waveData.RemainTime);
        }
    }

    private void OnComplete()
    {
        isGameComplete = true;

        // 웨이브를 최종 클리어 했을 시
        Debug.Log("Clear");
    }
}