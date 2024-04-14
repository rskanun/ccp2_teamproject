using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoPanel : MonoBehaviour
{
    public GameObject userMenuPanel;
    public GameObject askGiveAdminPanel;
    public GameObject askKickUserPanel;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void toggleUserMenuPanel()       //누르면 유저 메뉴 패널 여닫기
    {
        userMenuPanel.SetActive(!userMenuPanel.activeSelf);
    }
    public void openAskGiveAdminPanel()     //방장 넘겨주기 패널 열기
    {
        askGiveAdminPanel.SetActive(true);
    }
    public void openAskKickUserPanel()     //유저 강퇴 패널 열기
    {
        askKickUserPanel.SetActive(true);
    }

}
