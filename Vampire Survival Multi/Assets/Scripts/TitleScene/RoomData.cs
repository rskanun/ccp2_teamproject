using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableData/Room Data", order = int.MaxValue)]
public class RoomData : ScriptableObject
{
    [SerializeField]
    private string roomName;
    public string RoomName { get { return roomName; } }

    [SerializeField]
    private int maxPeopleNum;
    public int MaxPeopleNum { get { return maxPeopleNum; } }

    [SerializeField]
    private bool isPrivate;
    public bool IsPrivate { get { return isPrivate; } }

    [SerializeField]
    private string password;
    public string Password { get { return password; } }

    [SerializeField]
    private string roomCode;
    public string RoomCode { get { return roomCode; } }

}
