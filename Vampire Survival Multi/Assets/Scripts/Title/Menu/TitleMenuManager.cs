using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TitleMenuUI))]
public class TitleMenuManager : MonoBehaviourPunCallbacks
{
    // 서버 관련 변수
    private string gameVersion = "1.0.0a";

    // 참조 컴포넌트
    public GameObject settingPanel;

    // UI
    private TitleMenuUI ui;

    [Header("참조 스크립트")]
    [SerializeField] private MultiRoomManager roomManager;

    private void Awake()
    {
        ui = GetComponent<TitleMenuUI>();
    }

    /***************************************************************
    * [ 멀티 접속 ]
    * 
    * 매치메이킹 서버 접속과 방 목록 띄우기
    ***************************************************************/

    public void OnClickMultiPlay()
    {
        ConnectServer();
    }

    private void ConnectServer()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        // 연결 알림창
        ui.SetConnectiongPanel(true);
    }

    public override void OnConnectedToMaster()
    {
        // 연결 알림창 제거
        ui.SetConnectiongPanel(false);

        // 멀티 방 목록 띄우기
        roomManager.OpenMultiRoomList();
    }

    public void openSettingPanel() 
    {
        settingPanel.SetActive(true);
    }

    public void loadLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
