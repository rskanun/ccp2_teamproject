using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("참조 스크립트")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Confirm resultConfirm;

    [Header("시작 위치")]
    [SerializeField] private List<Vector2> startPoints;

    // 플레이어 리소스
    private GameObject playerPrefab;
    private List<PlayerData> playerDatas;

    // 참조 데이터
    private WaveData waveData;
    private GameData gameData;

    private void Awake()
    {
        waveData = WaveData.Instance;
        gameData = GameData.Instance;

        // Init Player Resource
        PlayerResource resource = PlayerResource.Instance;

        playerPrefab = resource.PlayerPrefab;
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
        // 웨이브 시작
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
                GameObject playerObj = Instantiate(playerPrefab, startPoints[i], Quaternion.identity);

                // 플레이어 오브젝트에 데이터 부여
                Player player = playerObj.GetComponent<Player>();
                player.InitPlayerData(playerData);

                // 플레이어 오브젝트 목록에 추가
                playerList.Add(playerObj);
            }
        }

        GameData.Instance.InitData(playerList);
    }

    private void Update()
    {
        // 플레이어 상태 체크
        UpdatePlayersStatus();

        // 웨이브 상태 진행
        UpdateWaveStatus();
    }

    /***************************************************************
    * [ 웨이브 진행 ]
    * 
    * 웨이브 진행에 따른 게임 진행 설정
    ***************************************************************/

    private void UpdateWaveStatus()
    {
        if (waveData.IsRunning)
        {
            // 남은 시간이 없거나, 모든 몬스터를 죽였을 경우
            if (waveData.RemainTime <= 0 || waveData.MobCount <= 0)
            {
                // 최종 웨이브가 아니고, 시간이 남은 경우
                if (waveData.RemainTime > 0 && waveData.IsLastWave == false)
                {
                    // 클리어 보상
                    OnWaveClear();
                }

                // 다음 웨이브 진행
                waveData.NextWave();

                // 만약 웨이브가 종료된 경우
                if (waveData.IsRunning == false)
                {
                    // 게임 종료
                    OnGameClear();
                }
            }
            else
                waveData.RemainTime -= Time.deltaTime;
        }
    }

    private void OnWaveClear()
    {
        Debug.Log("Wave Clear");


    }

    private void OnRevive()
    {
        
        
        gameData.ReviveAllPlayer();
    }

    private void UpdatePlayersStatus()
    {
        if (gameData.IsAllDead && waveData.IsRunning)
        {
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        waveData.IsRunning = false;

        Time.timeScale = 0.0f;
        resultConfirm.Active("Game Over...", () =>
        {
            PhotonNetwork.LeaveRoom();

            Time.timeScale = 1.0f;
        });
    }

    private void OnGameClear()
    {
        // 웨이브를 최종 클리어 했을 시
        Time.timeScale = 0.0f;
        resultConfirm.Active("Game Clear!!", () =>
        {
            PhotonNetwork.LeaveRoom();

            Time.timeScale = 1.0f;
        });
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("TitleScene");
    }
}