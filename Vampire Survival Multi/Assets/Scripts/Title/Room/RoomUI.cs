using Photon.Realtime;
using TMPro;
using UnityEngine;

public enum RoomType
{
    Public,
    Private,
    Hidden
}
public class RoomUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI maxPlayer;
    [SerializeField] private TextMeshProUGUI playerCount;
    [SerializeField] private TextMeshProUGUI roomType;

    public void InitRoom(RoomInfo room)
    {
        title.text = (string)room.CustomProperties["RoomName"];
        maxPlayer.text = room.MaxPlayers.ToString();
        playerCount.text = room.PlayerCount.ToString();
        roomType.text = RoomTypeToString((RoomType)room.CustomProperties["RoomType"]);
    }

    private string RoomTypeToString(RoomType type)
    {
        switch (type)
        {
            case RoomType.Public: return "공개";
            case RoomType.Private: return "비공개";
            default: return "";
        }
    }
}