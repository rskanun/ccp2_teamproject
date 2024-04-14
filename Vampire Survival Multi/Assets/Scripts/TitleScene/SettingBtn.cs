using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBtn : MonoBehaviour
{
    public GameObject settingPanel;
    public void showSetting()
    {
        settingPanel.SetActive(true);
    }
}
