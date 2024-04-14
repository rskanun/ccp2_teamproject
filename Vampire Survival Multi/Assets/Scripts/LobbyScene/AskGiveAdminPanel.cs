using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskGiveAdminPanel : MonoBehaviour
{
    public GameObject askGiveAdminPanel;
    public GameObject userMenuPanel;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void closeAskGiveAdminPanel()
    {
        askGiveAdminPanel.SetActive(false);
        userMenuPanel.SetActive(false);
    }
}
