using UnityEngine;

public class MultiRoomUI : MonoBehaviour
{
    [Header("방 목록")]
    [SerializeField] private GameObject roomList;

    public void SetRoomList(bool isActive)
    {
        roomList.SetActive(isActive);
    }
}