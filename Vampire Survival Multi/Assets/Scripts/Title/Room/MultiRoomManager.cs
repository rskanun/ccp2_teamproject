using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MultiRoomUI))]
public class MultiRoomManager : MonoBehaviourPunCallbacks
{
    [Header("참조 스크립트")]
    [SerializeField] private MultiRoomUI ui;
    [SerializeField] private PhotonManager photonManager;

    private static int index = 1;

    public void OnClickMultiPlay()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            photonManager.ConnectServer();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        ui.SetRoomList(true);
    }

    public void OnExit()
    {
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby()
    {
        ui.SetRoomList(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("JOIN!");
    }

    /***************************************************************
    * [ 방 목록 ]
    * 
    * 현재 개설된 방 목록 띄우기
    ***************************************************************/

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            ui.AddRoomObj(room);
        }
    }

    /***************************************************************
    * [ 방 생성 ]
    * 
    * 새로운 방 개설
    ***************************************************************/

    public void CreateNewRoom()
    {
        RoomOptions options = ui.CreateRoom();

        PhotonNetwork.CreateRoom(null, options, null);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("create");
    }
}