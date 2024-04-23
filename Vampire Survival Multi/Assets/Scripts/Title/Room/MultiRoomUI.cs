using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class MultiRoomUI : MonoBehaviour
{
    [Header("참조 오브젝트")]
    [SerializeField] private GameObject roomList;
    [SerializeField] private GameObject createPanel;
    [SerializeField] private GameObject roomContainer;

    [Header("방 생성 정보")]
    [SerializeField] private TMP_Dropdown maxPlayerDropdown;
    [SerializeField] private TMP_Dropdown roomTypeDropdown;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField nameInput;

    [Header("생성될 방 프리팹")]
    [SerializeField] private GameObject roomPrefab;

    /***************************************************************
    * [ 방 생성 ]
    * 
    * 방 생성에 관련된 UI 함수
    ***************************************************************/

    public void SetCreatePanel(bool isActive)
    {
        createPanel.SetActive(isActive);
    }

    public RoomOptions CreateRoom()
    {
        Hashtable properties = new Hashtable();

        string[] keys = new string[3] { "RoomName", "RoomType", "Password" };

        properties.Add(keys[0], GetRoomName());
        properties.Add(keys[1], GetRoomType(roomTypeDropdown.value));
        properties.Add(keys[2], GetPassword());

        RoomOptions options = new RoomOptions();

        options.MaxPlayers = GetMaxPlayer(maxPlayerDropdown.value);
        options.IsVisible = (properties[keys[1]].Equals(RoomType.Hidden) == false);
        options.CustomRoomProperties = properties;
        options.CustomRoomPropertiesForLobby = keys;

        return options;
    }

    private string GetRoomName()
    {
        return nameInput.text;
    }

    private int GetMaxPlayer(int index)
    {
        string maxPlayer = maxPlayerDropdown.options[index].text;

        return int.Parse(maxPlayer);
    }

    private RoomType GetRoomType(int index)
    {
        return (RoomType)index;
    }

    private string GetPassword()
    {
        return passwordInput.text;
    }

    /***************************************************************
    * [ 방 목록 ]
    * 
    * 방 목록에 관한 UI 함수
    ***************************************************************/

    public void SetRoomList(bool isActive)
    {
        roomList.SetActive(isActive);
    }

    public void AddRoomObj(RoomInfo info)
    {
        GameObject roomObj = Instantiate(roomPrefab, roomContainer.transform);

        RoomUI roomUI = roomObj.GetComponent<RoomUI>();
        roomUI.InitRoom(info);
    }
}