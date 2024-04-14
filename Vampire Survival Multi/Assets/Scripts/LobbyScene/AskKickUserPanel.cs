using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskKickUserPanel : MonoBehaviour
{
    public GameObject askKickUserPanel;
    public GameObject userMenuPanel;
    public void closeAskKickUserPanel()
    {
        askKickUserPanel.SetActive(false);
        userMenuPanel.SetActive(false);
    }
}
