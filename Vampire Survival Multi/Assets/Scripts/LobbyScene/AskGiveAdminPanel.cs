using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskGiveAdminPanel : MonoBehaviour
{
    public GameObject askGiveAdminPanel;
    public GameObject userMenuPanel;
    public void closeAskGiveAdminPanel()
    {
        askGiveAdminPanel.SetActive(false);
        userMenuPanel.SetActive(false);
    }
}
