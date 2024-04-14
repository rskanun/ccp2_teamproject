using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AskEnterPanel : MonoBehaviour
{
    [SerializeField]
    public GameObject askEnterPanelPrefab;
    [SerializeField]
    public GameObject roomInfoPrefab;
    public TextMeshProUGUI tryPasswordTxt;
    private string publicTxt = "공개";
    private string privateTxt = "비공개";

    public void closeAskEnterPanel()        //방 진입 물어보는 패널 닫기
    {
        askEnterPanelPrefab.SetActive(false);
    }

    public void checkPassword()         //패스워드 확인
    {
        string tryPw = tryPasswordTxt.text.Replace("\u200B", "");
        string pw = roomInfoPrefab.transform.Find("PasswordTxt").GetComponent<TextMeshProUGUI>().text;
        string isPrivateTxt = roomInfoPrefab.transform.Find("IsPrivateImg/IsPrivateTxt").GetComponent<TextMeshProUGUI>().text;

        if (isPrivateTxt == publicTxt)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else if (isPrivateTxt.Equals(privateTxt) && tryPw.Equals(pw))
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            Debug.Log("틀린 비밀번호 ==> 입장 불가능");
        }
    }
}
