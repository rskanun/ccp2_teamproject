﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private PlayerTracker cameraManager;

    [Header("시작 위치")]
    [SerializeField] private List<Vector2> startPoints;

    // 플레이어 리소스
    private GameObject playerPrefab;
    private GameObject localPlayerPrefab;
    private List<PlayerData> playerDatas;

    // 참조 데이터
    private WaveData waveData;

    private bool isGameComplete = false;

    private void Awake()
    {
        waveData = WaveData.Instance;

        // Init Player Resource
        PlayerResource resource = PlayerResource.Instance;

        playerPrefab = resource.PlayerPrefab;
        localPlayerPrefab = resource.LocalPlayerPrefab;
        playerDatas = resource.PlayerDatas;
    }

    private void Start()
    {
        // 게임 데이터 초기화
        InitGameData();

        // 장비 초기 셋팅
        PlayerEquip.Instance.InitEquips();

        // 게임 시작
        GameStart();
    }

    private void InitGameData()
    {
        // 플레이어 데이터 초기화
        InitPlayer();
    }

    private void GameStart()
    {
        waveData.IsRunning = true;
    }

    /***************************************************************
    * [ 플레이어 세팅 ]
    * 
    * 게임 시작 시 플레이어에 관한 세팅
    ***************************************************************/

    private void InitPlayer()
    {
        // 게임을 플레이하는 플레이어 오브젝트 목록
        List<GameObject> playerList = new List<GameObject>();

        for(int i = 0; i < playerDatas.Count; i++)
        {
            PlayerData playerData = playerDatas[i];

            if (playerData.IsPlaying)
            {
                GameObject playerObj = SpawnPlayer(playerData, startPoints[i]);

                // 플레이어 오브젝트에 데이터 부여
                Player player = playerObj.GetComponent<Player>();
                player.InitPlayerData(playerData);

                // 플레이어 오브젝트 목록에 추가
                playerList.Add(playerObj);
            }
        }

        GameData.Instance.InitData(playerList);
    }

    private GameObject SpawnPlayer(PlayerData playerData, Vector2 spawnPoint)
    {
        if (playerData == LocalPlayerData.Instance.PlayerData)
        {
            // 로컬 플레이어 오브젝트 생성
            GameObject playerObj = Instantiate(localPlayerPrefab, spawnPoint, Quaternion.identity);

            InitLocalPlayer(playerObj);

            return playerObj;
        }
        else
        {
            // 일반 플레이어 오브젝트 생성
            GameObject playerObj = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);

            return playerObj;
        }
    }

    private void InitLocalPlayer(GameObject localPlayerObj)
    {
        // 카메라 설정
        cameraManager.SetPlayer(localPlayerObj.transform);
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
            // 웨이브 종료 조건을 충족했을 경우
            if (waveManager.IsWaveEnded)
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
            waveManager.UpdateTimer();
        }
    }

    private void OnComplete()
    {
        isGameComplete = true;

        // 웨이브를 최종 클리어 했을 시
        Debug.Log("Clear");
    }
}