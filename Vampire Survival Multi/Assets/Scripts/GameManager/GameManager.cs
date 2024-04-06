using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("초기 클래스")]
    [SerializeField] private ClassData classData;

    private void Start()
    {
        // 게임 데이터 초기화
        InitGameData();

        // 초기 클래스 설정
        LocalPlayerData.Instance.Class = classData;

        // 장비 초기 셋팅
        PlayerEquip.Instance.InitEquips();
    }

    private void InitGameData()
    {
        // 게임을 플레이 할 플레이어 목록 가져오기
        List<PlayerData> playerDatas = new List<PlayerData>();

        playerDatas.Add(LocalPlayerData.Instance);

        GameData.Instance.InitData(playerDatas);
    }
}