using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelManager : MonoBehaviour
{
    [SerializeField] private List<PlayerPanelUI> panels;

    private void Start()
    {
        // 0번째 칸에 플레이어 배치
        OnClickMove(0);
    }

    public void OnClickMove(int index)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            PlayerPanelUI panelUI = panels[i];

            if (i == index) panelUI.OnPanelEnter();
            else panelUI.OnPanelLeave();
        }
    }

    public void OnClickAdd(int index)
    {
        panels[index].OnAddPlayer();
    }

    public void OnClickRemove(int index)
    {
        panels[index].OnRemovePlayer();
    }
}