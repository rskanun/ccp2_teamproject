using UnityEngine;

[RequireComponent (typeof(MultiRoomUI))]
public class MultiRoomManager : MonoBehaviour
{
    // 참조 스크립트
    private MultiRoomUI ui;

    private void Start()
    {
        ui = GetComponent<MultiRoomUI>();
    }

    public void OpenMultiRoomList()
    {
        ui.SetRoomList(true);
    }
}