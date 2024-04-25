using ExitGames.Client.Photon;
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

    private void Start()
    {
        // Init UI
        RoomInfo room = PhotonNetwork.CurrentRoom;

        ui.SetRoomCode(room.Name);
        SetPlayerPanel(room.MaxPlayers);
        UpdateReadyOrStartButton();

        // Init Panel
        if (PhotonNetwork.IsMasterClient)
        {
            // 나머지는 방장이 할당
            SetMyPanelManager(playerPanels[0]);
        }
    }

    private void SetPlayerPanel(int num)
    {
        for (int i = 0; i < playerPanels.Count; i++)
        {
            PlayerPanelManager panel = playerPanels[i];

            panel.OnExitPlayer();

            if (i < num) playerPanels[i].IsClosed = false;
            else playerPanels[i].IsClosed = true;
        }
    }

    private void UpdateReadyOrStartButton()
    {
        if (PhotonNetwork.IsMasterClient) ui.ChangeToStartButton();
        else ui.ChangeToReadyButton();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PlayerPanelManager manager = GetEmptyPanel();

        photonView.RPC(nameof(SetMyPanelManager), newPlayer, manager);
    }

    public override void OnLeftRoom()
    {
        myPanelManager.OnExitPlayer();
    }

    private PlayerPanelManager GetEmptyPanel()
    {
        foreach (PlayerPanelManager manager in  playerPanels)
        {
            if (manager.IsExist == false)
            {
                return manager;
            }
        }

        return null;
    }

    [PunRPC]
    private void SetMyPanelManager(PlayerPanelManager manager)
    {
        myPanelManager = manager;
        myPanelManager.SetInfo(PhotonNetwork.LocalPlayer);
    }

    /***************************************************************
    * [ 서버 연결 ]
    * 
    * 서버 연결이 끊겼을 경우의 조치
    ***************************************************************/

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause != DisconnectCause.ApplicationQuit)
        {
            ui.SetDisconnectedConfirm(true);
        }
    }

    public void OnClickToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }
}