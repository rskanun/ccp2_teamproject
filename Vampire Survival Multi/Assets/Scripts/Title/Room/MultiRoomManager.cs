using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(MultiRoomUI))]
public class MultiRoomManager : MonoBehaviourPunCallbacks
{
    [Header("참조 스크립트")]
    [SerializeField] private MultiRoomUI ui;
    [SerializeField] private PasswordManager passwordManager;
    [SerializeField] private CreateRoomManager createManager;
    [SerializeField] private PhotonManager photonManager;

    public void OnClickMultiPlay()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            photonManager.ConnectServer();
        }
        else
        {
            photonManager.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        // 로비에 있을 때에만 방 목록 보이기
        ui.SetRoomList(true);
    }

    public void OnExit()
    {
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby()
    {
        // 로비에서 나가면 방 목록 제거
        ui.SetRoomList(false);
    }

    public override void OnJoinedRoom()
    {
        // 방 연결 시 로비창으로 이동
        SceneManager.LoadScene("LobbyScene");
    }

    /***************************************************************
    * [ 방 목록 ]
    * 
    * 현재 개설된 방 목록 표시 및 입장 이벤트
    ***************************************************************/

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            ui.AddRoomObj(room, (id, roomPassword) => OnEnterRoom(id, roomPassword));
        }
    }

    public void OnEnterRoom(string id, string roomPassword)
    {
        if (roomPassword != "")
        {
            // 비밀번호가 걸린 방이면 비밀번호 입력시키기
            passwordManager.InputPassword(roomPassword, () =>
            {
                // 비밀번호 통과했을 경우 방 입장
                PhotonNetwork.JoinRoom(id);
            });
        }
        else
        {
            // 비밀번호가 걸리지 않은 방은 바로 입장
            PhotonNetwork.JoinRoom(id);
        }
    }

    public void OnClickSearch()
    {
        // 검색창에 입력된 단어를 토대로 방 검색
        string keyword = ui.GetSearchKeyword();

        SearchRoom(keyword);
    }

    private void SearchRoom(string keyword)
    {
        // keyword가 포함된 방 목록 검색
        string sqlQuery = $"SELECT * FROM PhotonRoomProperties WHERE RoomName LIKE '%{keyword}%'";
        TypedLobby lobby = PhotonNetwork.CurrentLobby;

        PhotonNetwork.GetCustomRoomList(lobby, sqlQuery);
    }
}