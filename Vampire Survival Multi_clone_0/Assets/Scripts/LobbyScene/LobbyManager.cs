using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("참조 스크립트")]
    [SerializeField] private LobbyUI ui;

    [Header("플레이어 패널 스크립트")]
    [SerializeField] 
    private List<PlayerPanelManager> playerPanels;
    private PlayerPanelManager myPanelManager;

    // 해당 방의 호스트
    private Photon.Realtime.Player masterClient;

    /***************************************************************
    * [ 방 입장 ]
    * 
    * 방 입장 시 이벤트 설정
    ***************************************************************/

    private void Start()
    {
        // 방장 설정
        masterClient = PhotonNetwork.MasterClient;

        // 로비 기본 UI 처리
        InitLobbyUI();

        // 첫 번째 자리에 플레이어 할당
        if (PhotonNetwork.IsMasterClient)
        {
            // 나머지는 OnPlayerEnteredRoom에서 방장이 할당
            SetMyPanelManager(0);
            myPanelManager.SetPanelInfo(PhotonNetwork.LocalPlayer);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // 방장이 새로 들어온 플레이어에게 패널 할당
        if (masterClient.IsLocal)
        {
            // 빈 패널에 플레이어 할당
            SetPlayerPanel(newPlayer);
        }
    }

    private void SetPlayerPanel(Photon.Realtime.Player newPlayer)
    {
        // 빈 패널 찾기
        int index = GetEmptyPanelIndex();
        PlayerPanelManager manager = playerPanels[index];

        // 새로 들어온 플레이어에게 할당
        photonView.RPC(nameof(SetMyPanelManager), newPlayer, index);

        // 모든 플레이어에게 해당 패널 설정
        manager.OnJoinPlayer(newPlayer);

        // 현재 패널 정보 동기화
        SynchroPanel(newPlayer);
    }

    /***************************************************************
    * [ 방 퇴장 ]
    * 
    * 방 퇴장 시 이벤트 설정
    ***************************************************************/

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (masterClient.IsLocal)
        {
            RemovePlayer(otherPlayer);
        }
    }

    public override void OnLeftRoom()
    {
        // 방을 나갔으면 타이틀로 돌아가기
        SceneManager.LoadScene("TitleScene");
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        // 새로운 방장은 이전 방장의 권한을 받아 UI 수정
        if (newMasterClient.IsLocal)
        {
            // 이전 방장을 패널에서 삭제
            RemovePlayer(masterClient);

            // 방장 전용 UI로 변경
            SetActivePlayerMenu();
            UpdateReadyOrStartButton();

            // 방장 넘기기
            masterClient = newMasterClient;
        }
    }

    private void RemovePlayer(Photon.Realtime.Player removePlayer)
    {
        foreach (PlayerPanelManager manager in playerPanels)
        {
            Photon.Realtime.Player player = manager.JoinPlayer;

            if (player == removePlayer)
            {
                manager.OnExitPlayer();
            }
        }
    }

    /***************************************************************
    * [ 서버 통신 ]
    * 
    * 리슨 서버 형식(방장이 서버 역할)의 통신 구축
    ***************************************************************/

    [PunRPC]
    private void SetMyPanelManager(int index)
    {
        // 패널 할당 및 설정
        PlayerPanelManager manager = playerPanels[index];

        myPanelManager = manager;
    }

    private void SynchroPanel(Photon.Realtime.Player newPlayer)
    {
        // 모든 패널 동기화
        foreach (PlayerPanelManager manager in playerPanels)
        {
            Photon.Realtime.Player panelPlayer = manager.JoinPlayer;

            manager.SendData(newPlayer, panelPlayer);
        }
    }

    private void SetActivePlayerMenu()
    {
        Debug.Log("Set Player Menu");
        foreach (PlayerPanelManager manager in playerPanels)
        {
            manager.SetActivePlayer();
        }
    }

    /***************************************************************
    * [ UI 설정 ]
    * 
    * 상황에 따른 UI 설정
    ***************************************************************/

    private void InitLobbyUI()
    {
        // 로비 기본 UI 설정
        RoomInfo room = PhotonNetwork.CurrentRoom;

        ui.SetRoomCode(room.Name);
        SetPlayerPanel(room.MaxPlayers);
        UpdateReadyOrStartButton();
    }

    private void SetPlayerPanel(int num)
    {
        // 인원수만큼 패널 활성화
        for (int i = 0; i < playerPanels.Count; i++)
        {
            PlayerPanelManager panel = playerPanels[i];

            // 패널 초기화
            panel.InitUI();

            if (i < num) playerPanels[i].IsClosed = false;
            else playerPanels[i].IsClosed = true;
        }
    }

    private void UpdateReadyOrStartButton()
    {
        // 게임준비 버튼 및 시작 버튼 활성화
        if (masterClient.IsLocal) ui.ChangeToStartButton();
        else ui.ChangeToReadyButton();
    }

    private int GetEmptyPanelIndex()
    {
        // 가장 가까운 빈 패널 찾기
        for (int i = 0; i <= playerPanels.Count; i++)
        {
            PlayerPanelManager manager = playerPanels[i];

            if (manager.IsExist == false)
            {
                return i;
            }
        }

        return -1;
    }

    /***************************************************************
    * [ 서버 연결 ]
    * 
    * 서버 연결이 끊겼을 경우의 조치
    ***************************************************************/

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 게임을 나가서 생긴 끊김이 아닐 경우
        if (cause != DisconnectCause.ApplicationQuit)
        {
            // 오류 출력 패널 활성화
            ui.SetDisconnectedConfirm(true);
        }
    }

    public void OnClickToTitle()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }
}