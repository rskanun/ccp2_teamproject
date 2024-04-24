using Photon.Realtime;
using TMPro;
using UnityEngine;

public class MultiRoomUI : MonoBehaviour
{
    [Header("참조 오브젝트")]
    [SerializeField] private GameObject roomContainer;
    [SerializeField] private GameObject roomList;
    [SerializeField] private GameObject createPanel;
    [SerializeField] private TMP_InputField searchField;

    [Header("생성될 방 프리팹")]
    [SerializeField] private GameObject roomPrefab;

    /***************************************************************
    * [ 방 목록 ]
    * 
    * 방 목록에 관한 UI 함수
    ***************************************************************/

    public void SetRoomList(bool isActive)
    {
        roomList.SetActive(isActive);
    }

    public void AddRoomObj(RoomInfo info, RoomManager.SelectedCallback listener)
    {
        GameObject roomObj = Instantiate(roomPrefab, roomContainer.transform);

        RoomManager room = roomObj.GetComponent<RoomManager>();
        room.InitRoomInfo(info);
        room.SetClickHandler(listener);
    }

    public string GetSearchKeyword()
    {
        return searchField.text;
    }

    public void ClearSearchField()
    {
        searchField.text = "";
    }

    /***************************************************************
    * [ 방 생성 ]
    * 
    * 방 생성 패널 활성화 여부 설정
    ***************************************************************/

    public void SetCreatePanel(bool isActive)
    {
        createPanel.SetActive(isActive);
    }
}