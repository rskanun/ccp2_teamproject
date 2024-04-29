using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SearchCodePanel : MonoBehaviour
{
    public GameObject roomCodePanel;
    [SerializeField]
    private TMP_InputField searchCodeField;
    public TextMeshProUGUI messageText;
    public float displayDuration = 3f;

    public void openSearchCodePanel()
    {
        roomCodePanel.SetActive(true);
    }

    public void closeSearchCodePanel()
    {
        roomCodePanel.SetActive(false);
    }

    public void enterByCode()
    {
        string roomCode = searchCodeField.text;
        if (PhotonNetwork.CountOfRooms > 0)
        {
            PhotonNetwork.JoinRoom(roomCode);
        }
        else
        {
            ShowText();
            Invoke("HideText", displayDuration);
        }
    }

    public void ClearSearchField()
    {
        searchCodeField.text = "";
    }

    void ShowText()
    {
        messageText.gameObject.SetActive(true);
    }

    void HideText()
    {
        messageText.gameObject.SetActive(false);
    }
}