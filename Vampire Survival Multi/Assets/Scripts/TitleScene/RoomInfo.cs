using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomInfo : MonoBehaviour
{
    public GameObject askEnterPanel;
    public TextMeshProUGUI isPrivateTxt;
    private string publicTxt = "공개";


    private void Awake()
    {
        askEnterPanel.transform.position = new Vector3(0f, 0f, askEnterPanel.transform.position.z);
    }
    public void enterRoom()         //방 진입 물어보기
    {
        GameObject multiSearchPanel = GameObject.Find("MultiSearchPanel");
        GameObject newAskEnterPanel = Instantiate(askEnterPanel, multiSearchPanel.transform);
        newAskEnterPanel.SetActive(true);

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
