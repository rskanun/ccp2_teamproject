using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMultiBtn : MonoBehaviour
{
    public GameObject multiPanel;

    public void showMultiPanel()
    {
        multiPanel.SetActive(true);
    }
}
