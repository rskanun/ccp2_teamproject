using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject characterSettingPanel;
    public GameObject roomSettingPanel;
    public Button[] buttons;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void openCharacterSettingPanel()     //캐릭터 패널 열기
    {
        characterSettingPanel.SetActive(true);
    }

    public void openRoomSettingPanel()      //방 설정 패널 열기
    {
        roomSettingPanel.SetActive(true);
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
    public void exitScene()                 //타이틀 씬으로 돌아가기
    {
        SceneManager.LoadScene("TitleScene");
    }

    
}
