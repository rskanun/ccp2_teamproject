using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [Header("로비 패널 목록")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject classSelectPanel;

    [Header("알림 패널")]
    [SerializeField] private GameObject disconnectConfirm;
    [SerializeField] private GameObject transferAdminConfirm;
    [SerializeField] private GameObject kickConfirm;

    [Header("참조 오브젝트")]
    [SerializeField] private TextMeshProUGUI roomCodeText;
    [SerializeField] private TextMeshProUGUI readyOrStartText;

    /***************************************************************
    * [ 로비 UI ]
    * 
    * 로비 내 UI 변경
    ***************************************************************/

    public void SetDisconnectedConfirm(bool isActive)
    {
        disconnectConfirm.SetActive(isActive);
    }

    public void SetRoomCode(string code)
    {
        roomCodeText.text = code;
    }

    public void ChangeToStartButton()
    {
        readyOrStartText.text = "게임 시작";
    }

    public void ChangeToReadyButton()
    {
        readyOrStartText.text = "게임 준비";
    }
}