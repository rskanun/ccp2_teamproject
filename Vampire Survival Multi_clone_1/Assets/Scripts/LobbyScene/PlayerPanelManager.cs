using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PlayerPanelUI))]
public class PlayerPanelManager : MonoBehaviourPun
{
    [Header("참조 스크립트")]
    [SerializeField] private PlayerPanelUI ui;

    // 플레이어 정보
    private ClassData classData;
    private Photon.Realtime.Player _joinPlayer;
    public Photon.Realtime.Player JoinPlayer
    {
        get { return _joinPlayer; }
    }

    // 패널 정보
    private bool _isClosed;
    public bool IsClosed
    {
        get { return _isClosed; }
        set
        {
            _isClosed = value;

            ui.SetActiveCloseMark(value);
        }
    }

    public bool IsExist
    {
        get { return _joinPlayer != null; }
    }

    public void InitUI()
    {
        ui.SetActiveCharacter(false);
        ui.SetActivePlayerMenu(false);
        ui.SetClassName("");
    }

    /***************************************************************
    * [ 패널 입장 ]
    * 
    * 방 입장 후 패널 설정
    ***************************************************************/

    public void OnJoinPlayer(Photon.Realtime.Player player)
    {
        // 모든 플레이어에게 해당 패널 설정
        photonView.RPC(nameof(SetPanelInfo), RpcTarget.All, player);
    }

    [PunRPC]
    public void SetPanelInfo(Photon.Realtime.Player player)
    {
        _joinPlayer = player;

        // 본인인 경우 클래스 초기 설정
        if (player.IsLocal)
        {
            // 클래스 목록 중 가장 처음 클래스 등록
            SetInitClass();
        }

        // UI 설정
        SetActivePlayer();
    }

    public void SetActivePlayer()
    {
        if (IsExist && IsClosed == false)
        {
            if (PhotonNetwork.IsMasterClient && !JoinPlayer.IsLocal)
            {
                // 방장은 해당 플레이어 조작 메뉴 포함 전부 활성화
                ui.SetActiveCharacter(true);
                ui.SetActivePlayerMenu(true);
            }
            else
            {
                // 방장 외엔 플레이터 정보 활성화
                ui.SetActiveCharacter(true);
                ui.SetActivePlayerMenu(false);
            }
        }
    }

    /***************************************************************
    * [ 패널 퇴장 ]
    * 
    * 방 퇴장 후 패널 설정
    ***************************************************************/

    public void OnExitPlayer()
    {
        // 해당 패널을 모든 플레이어에 대해 초기화
        photonView.RPC(nameof(ResetPanel), RpcTarget.All);
    }

    [PunRPC]
    private void ResetPanel()
    {
        _joinPlayer = null;

        // UI 초기화
        InitUI();
    }

    /***************************************************************
    * [ 직업 할당 ]
    * 
    * 해당 패널의 플레이어의 직업 할당
    ***************************************************************/

    private void SetInitClass()
    {
        ClassData initClass = ClassResource.Instance.ClassList[0];

        SetClass(initClass);
    }

    public void SetClass(ClassData classData)
    {
        // 결정한 직업을 방장을 통해 설정
        photonView.RPC(nameof(SelectedClass), RpcTarget.MasterClient, classData.ID);
    }

    [PunRPC]
    private void SelectedClass(int classID)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 다른 사람들도 적용시키기
            photonView.RPC(nameof(SelectedClass), RpcTarget.Others, classID);
        }

        // 해당 직업 번호를 토대로 직업 찾기
        ClassData classData = ClassResource.Instance.FindClass(classID);

        // 직업 적용
        this.classData = classData;
        SetClassName(classData.Name);
    }

    [PunRPC]
    private void SetClassName(string name)
    {
        ui.SetClassName(name);
    }

    /***************************************************************
    * [ 데이터 동기화 ]
    * 
    * 패널 상태 동기화
    ***************************************************************/

    public void SendData(Photon.Realtime.Player sendPlayer, Photon.Realtime.Player panelPlayer)
    {
        // 본인의 패널이 아닌 플레이어가 존재하는 패널만 동기화
        if (IsExist && _joinPlayer != sendPlayer)
        {
            photonView.RPC(nameof(ReceiveData), sendPlayer, panelPlayer, classData.ID);
        }
    }

    [PunRPC]
    private void ReceiveData(Photon.Realtime.Player player, int classID)
    {
        SetPanelInfo(player);
        SelectedClass(classID);
    }
}