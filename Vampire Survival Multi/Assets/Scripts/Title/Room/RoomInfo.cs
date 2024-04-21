using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomInfo : MonoBehaviour
{
    public GameObject askEnterPanel;        //방 입장 확인 패널
    public TextMeshProUGUI isPrivateTxt;    //방 공개 여부 텍스트
    public TextMeshProUGUI passwordTxt;    //방 비번 텍스트

    private string publicTxt = "공개";
    public static string selectedIsPrivateTxt;
    public static string selectedPW;

    void Awake()
    {
        //패널의 위치 잡기
        askEnterPanel.transform.position = new Vector3(0f, 0f, askEnterPanel.transform.position.z);
    }

    //방 진입 물어보기
    public void enterRoom()
    {
        GameObject multiSearchPanel = GameObject.Find("MultiSearchPanel");
        GameObject newAskEnterPanel = Instantiate(askEnterPanel, multiSearchPanel.transform);
        selectedIsPrivateTxt = isPrivateTxt.text;
        selectedPW = passwordTxt.text;

        if (isPrivateTxt.text.Equals(publicTxt))
        {
            Transform submitPasswordBarTransform = newAskEnterPanel.transform.Find("SubmitPasswordBar");
            if (submitPasswordBarTransform != null)
            {
                submitPasswordBarTransform.gameObject.SetActive(false);
            }
        }
    }
}
