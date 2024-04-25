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
        myPanelManager = InitMyPanel();
        myPanelManager.SetPlayer(PhotonNetwork.LocalPlayer);
    }

    private PlayerPanelManager InitMyPanel()
    {
        // 가장 앞에 있는 패널에 자리 지정
        foreach (PlayerPanelManager panel in playerPanels)
        {
            // 비어있는 패널에 자리 지정
            if (panel.IsExist == false)
            {
                return panel;
            }
        }

        return null;
    }

    private void SetPlayerPanel(int num)
    {
        for (int i = 0; i < playerPanels.Count; i++)
        {
            PlayerPanelManager panel = playerPanels[i];

            panel.InitUI();

            if (i < num) playerPanels[i].IsClosed = false;
            else playerPanels[i].IsClosed = true;
        }
    }

    private void UpdateReadyOrStartButton()
    {
        if (PhotonNetwork.IsMasterClient) ui.ChangeToStartButton();
        else ui.ChangeToReadyButton();
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