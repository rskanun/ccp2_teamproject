using Photon.Pun;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerPanelUI))]
public class PlayerPanelManager : MonoBehaviourPun, IPunObservable
{
    [Header("참조 스크립트")]
    [SerializeField] private PlayerPanelUI ui;

    // 플레이어 정보
    private ClassData classData;

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

    private bool _isExist;
    public bool IsExist
    {
        get { return _isExist; }
    }

    [PunRPC]
    public void SetInfo(Photon.Realtime.Player player)
    {
        _isExist = true;

        // 본인인 경우 클래스 초기 설정
        if (player.IsLocal)
        {
            // 클래스 목록 중 가장 처음 클래스 등록
            SetInitClass();

            // 다른 사람들도 적용
            photonView.RPC(nameof(SetInfo), RpcTarget.Others, player);
        }

        // UI 설정
        OnEnterPlayer();
    }

    public void OnEnterPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 방장은 해당 플레이어 조작 메뉴 포함 전부 활성화
            ui.SetActiveCharacter(true);
            ui.SetActivePlayerName(true);
            ui.SetActivePlayerMenu(true);
        }
        else
        {
            // 방장 외엔 메뉴 외 전부 활성화
            ui.SetActiveCharacter(true);
            ui.SetActivePlayerName(true);
            ui.SetActivePlayerMenu(false);
        }
    }

    public void OnExitPlayer()
    {
        _isExist = false;

        // UI 초기화
        ui.SetActiveCharacter(false);
        ui.SetActivePlayerName(false);
        ui.SetActivePlayerMenu(false);
        ui.SetClassName("");
    }

    private void SetInitClass()
    {
        ClassData initClass = ClassResource.Instance.ClassList[0];

        SetClass(initClass);
    }

    public void SetClass(ClassData classData)
    {
        this.classData = classData;

        // 결정한 직업 이름 설정
        photonView.RPC(nameof(ui.SetClassName), RpcTarget.All, classData.Name);
    }

    /***************************************************************
    * [ 데이터 동기화 ]
    * 
    * 패널 상태 동기화
    ***************************************************************/

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isExist);
            stream.SendNext(classData.ID);
        }
        else
        {
            _isExist = (bool)stream.ReceiveNext();

            // 플레이어가 존재하는 패널일 경우
            if (_isExist)
            {
                // 클래스 데이터 씌우기
                int id = (int)stream.ReceiveNext();
                classData = ClassResource.Instance.FindClass(id);

                SetClass(classData);
                OnEnterPlayer();
            }
            else
            {
                // 플레이어가 없다면 UI 초기화
                OnExitPlayer();
            }
        }
    }
}