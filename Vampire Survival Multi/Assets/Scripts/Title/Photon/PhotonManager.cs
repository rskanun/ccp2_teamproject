using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 서버 관련 변수
    private string gameVersion = "1.0.0a";

    // ui
    private PhotonUI ui;

    public delegate void OnConnectedServer();
    private event OnConnectedServer connectedCallBack;

    private void Awake()
    {
        ui = GetComponent<PhotonUI>();
    }

    /***************************************************************
    * [ 멀티 접속 ]
    * 
    * 매치메이킹 서버 접속
    ***************************************************************/

    public void ConnectServer(OnConnectedServer listener = null)
    {
        connectedCallBack = listener;

        if (PhotonNetwork.IsConnected == false)
        {
            // 연결 알림창 띄우기
            ui.SetConnectiongPanel(true);

            // 서버 연결
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        // 로비 접속
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ui.SetConnectiongPanel(false);
        ui.SetDisconnectedAlert(true);
    }

    public override void OnJoinedLobby()
    {
        // 연결 알림창 제거
        ui.SetConnectiongPanel(false);

        // 연결 후 이벤트 실행
        connectedCallBack?.Invoke();
    }
}