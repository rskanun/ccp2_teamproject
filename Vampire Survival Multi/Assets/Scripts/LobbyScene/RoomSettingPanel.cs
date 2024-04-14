using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSettingPanel : MonoBehaviour
{
    public GameObject roomSettingPanel;
    public Button[] buttons;
    public void closeRoomSettingPanel()
    {
        roomSettingPanel.SetActive(false);
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
}
