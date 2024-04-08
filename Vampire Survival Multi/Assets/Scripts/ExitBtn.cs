using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBtn : MonoBehaviour
{
    public GameObject currentPanel;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void closeCurrentPanel()
    {
        currentPanel.SetActive(false);
    }
}
