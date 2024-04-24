using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AskEnterController : MonoBehaviour
{
    //프리팹
    public GameObject askEnterPanelPrefab;      //방 입장 확인 패널의 프리팹
    public GameObject roomInfoPrefab;           //방 정보및 입장의 프리팹

    //방 입장 확인 내의 Objects
    public TextMeshProUGUI tryPasswordTxt;      //방 입장시 작성하는 비밀번호

    private string publicTxt = "공개";
    private string privateTxt = "비공개";

    public Button[] btnsActiveByAskEnter;       //방 진입 확인 패널 여닫기에 따라 활성화가 바뀌는 버튼들

    //방 진입 확인 패널 닫기 + 버튼 활성화 (AskEnterPanel프리팹 기능)
    public void closeAskEnterPanel()
    {
        askEnterPanelPrefab.SetActive(false);
        foreach (Button btns in btnsActiveByAskEnter)
        {
            btns.interactable = true;
        }
    }

    //패스워드 확인 및 입장
    public void checkPassword()
    {
        string tryPw = tryPasswordTxt.text.Replace("\u200B", "");

        string pw = RoomInfo2.selectedPW;
        string isPrivate = RoomInfo2.selectedIsPrivateTxt;

        if (isPrivate.Equals(publicTxt))  // 공개일 경우
        { 
            SceneManager.LoadScene("LobbyScene");
        }
        else if (isPrivate.Equals(privateTxt) && tryPw.Equals(pw))   // 비공개이지만 비번이 일치할 경우
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            Debug.Log("틀린 비밀번호 ==> 입장 불가능");
        }
    }

}
