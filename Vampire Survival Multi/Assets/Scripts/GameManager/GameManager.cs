using Photon.Pun;
using Photon.Realtime;
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
    private GameObject localPlayerPrefab;
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

        localPlayerPrefab = resource.LocalPlayerPrefab;
        playerDatas = resource.PlayerDatas;

        // Init Prefab in Photon Default Pool
        InitPrefabResource();
    }

    private void InitPrefabResource()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;

        if (pool != null)
        {
            pool.ResourceCache.Add(localPlayerPrefab.name, localPlayerPrefab);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 게임 데이터 초기화
            InitGameData();

            // 게임 시작
            GameStart();
        }
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
        GameData gameData = GameData.Instance;

        for(int i = 0; i < playerDatas.Count; i++)
        {
            PlayerData playerData = playerDatas[i];
            if (playerDatas[i].IsPlaying)
            {
                GameObject playerObj = SpawnPlayer(i, startPoints[i]);

                // 플레이어 오브젝트 목록에 추가
                gameData.AddPlayableChr(playerObj);
            }
        }

        gameData.InitData();
    }

    private GameObject SpawnPlayer(int index, Vector2 spawnPoint)
    {
        PlayerData playerData = playerDatas[index];

        // 로컬 플레이어 오브젝트 생성 및 제어권 설정
        GameObject playerObj = PhotonNetwork.Instantiate(localPlayerPrefab.name, spawnPoint, Quaternion.identity);
        PhotonView photonView = playerObj.GetComponent<PhotonView>();
        photonView.TransferOwnership(playerData.Player);

        // 카메라 설정
        // cameraManager.InitPlayer(playerObj);

        // 데이터 설정
        Player player = playerObj.GetComponent<Player>();
        player.InitPlayerData(index);

        return playerObj;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 플레이어 상태 체크
            UpdatePlayersStatus();

            // 웨이브 상태 진행
            UpdateWaveStatus();
        }
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
                    photonView.RPC(nameof(OnGameClear), RpcTarget.All);
                }
            }
            else
            {
                float time = Time.deltaTime;

                photonView.RPC(nameof(PassedWaveTime), RpcTarget.All, time);
            }
        }
    }

    [PunRPC]
    private void PassedWaveTime(float time)
    {
        waveData.RemainTime -= time;
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
        resultConfirm.OnActive("Game Over...", () =>
        {
            SceneManager.LoadScene("TitleScene");

            Time.timeScale = 1.0f;
        });
    }

    private void OnGameClear()
    {
        // 웨이브를 최종 클리어 했을 시
        Time.timeScale = 0.0f;
        resultConfirm.OnActive("Game Clear!!", () =>
        {
            SceneManager.LoadScene("TitleScene");

            Time.timeScale = 1.0f;
        });
    }

    /***************************************************************
    * [ 연결 끊김 ]
    * 
    * 연결이 끊겼을 때의 대처 방안
    ***************************************************************/

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        Time.timeScale = 0.0f;
        resultConfirm.OnActive("호스트의 서버 연결이 끊겼습니다!!", () =>
        {
            SceneManager.LoadScene("TitleScene");

            Time.timeScale = 1.0f;
        });
    }
}