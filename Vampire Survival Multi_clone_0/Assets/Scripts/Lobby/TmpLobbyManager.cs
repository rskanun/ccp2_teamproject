﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TmpLobbyManager : MonoBehaviour
{
    [SerializeField] private List<OldPlayerPanelUI> panels;

    // 플레이어 리소스
    private List<PlayerData> playerDatas;

    private void Start()
    {
        PlayerResource resource = PlayerResource.Instance;

        // Init Data
        playerDatas = resource.PlayerDatas;
    }

    public void OnClickStart()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            OldPlayerPanelUI panel = panels[i];
            PlayerData playerData = playerDatas[i];

            if (panel.State != PanelState.Nobody)
            {
                // 임시 직업 할당(직업 선택 창에서 할당)
                playerData.TestInitData();

                // 해당 자리의 플레이어 참가 설정
                playerData.IsPlaying = true;

                // 로컬 플레이어 설정
                if (panel.State == PanelState.InPlayer)
                {
                    LocalPlayerData localPlayer = LocalPlayerData.Instance;
                    localPlayer.InitPlayerData(playerData);
                }
            }
            else
            {
                // 플레이어 참가 해제
                playerData.IsPlaying = false;
            }
        }

        SceneLoadManager.LoadScene("InGame");
    }
}