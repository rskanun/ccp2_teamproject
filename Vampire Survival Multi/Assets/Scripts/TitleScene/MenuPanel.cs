using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    public GameObject multiPanel;
    public GameObject settingPanel;

    public void openMultiPanel()
    {
        multiPanel.SetActive(true);
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
