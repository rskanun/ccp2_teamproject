using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class MakeRoomPanel : MonoBehaviour
{
    public GameObject makeRoomPanel;
    public ScrollRect scrollView;
    public TMP_Dropdown roomPeopleNumDropdown;
    public TMP_Dropdown privateDropdown;
    public TMP_InputField passwordInputField;
    public TMP_InputField roomNameInputField;
    public GameObject roomPrefab;
    public int maxPasswordLimit = 8;
    public int maxRoomNameLimit = 12;
    private List<GameObject> roomList = new List<GameObject>();

    public TMP_InputField roomSearchBar;
    public Button multiExitBtn;
    public Button txtDeleteBtn;
    public Button makeRoomBtn;

    private string initialRoomPeopleNumValue;
    private string initialPrivateValue;
    private string initialPassword;
    private string initialRoomName;

    void Start()
    {
        initialRoomPeopleNumValue = roomPeopleNumDropdown.options[roomPeopleNumDropdown.value].text;
        initialPrivateValue = privateDropdown.options[privateDropdown.value].text;
        initialPassword = passwordInputField.text;
        initialRoomName = roomNameInputField.text;

        roomNameInputField.characterLimit = maxRoomNameLimit;
        roomNameInputField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(roomNameInputField, maxRoomNameLimit); });

        passwordInputField.characterLimit = maxPasswordLimit;
        passwordInputField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(passwordInputField, maxPasswordLimit); });
    }

    public void closeMakeRoomPanel()        //방 만들기 패널 닫기 + 원래의 입력했던 설정들 초기화
    {
        makeRoomPanel.SetActive(false);
        roomPeopleNumDropdown.value = roomPeopleNumDropdown.options.FindIndex(option => option.text == initialRoomPeopleNumValue);
        privateDropdown.value = privateDropdown.options.FindIndex(option => option.text == initialPrivateValue);
        passwordInputField.text = initialPassword;
        roomNameInputField.text = initialRoomName;

        multiExitBtn.interactable = true;
        txtDeleteBtn.interactable = true;
        makeRoomBtn.interactable = true;
        roomSearchBar.interactable = true;
    }

    public void enableMakePassword()        //비공개 시 비밀번호 생성 활성화, 공개시 비밀번호 생성 비활성화
    {
        if (privateDropdown.value == 1)
        {
            passwordInputField.interactable = true;
        }
        else
        {
            passwordInputField.interactable = false;
            passwordInputField.text = "";
        }
    }

    public void addRoomToList()
    {
        string roomPeopleNumValue = roomPeopleNumDropdown.options[roomPeopleNumDropdown.value].text;
        string privateValue = privateDropdown.options[privateDropdown.value].text;
        string password = passwordInputField.text;
        string roomName = roomNameInputField.text;

        if (roomName.Equals("") || (privateValue.Equals("비공개") && password.Equals("")))
        {
            Debug.Log("필수입력란 누락!");
        }
        else
        {
            GameObject newItem = Instantiate(roomPrefab, scrollView.content);

            newItem.transform.Find("RoomNameTxt").GetComponent<TMP_Text>().text = roomName;
            newItem.transform.Find("PasswordTxt").GetComponent<TMP_Text>().text = password;
            newItem.transform.Find("RoomPeopleNumImg/RoomPeopleNumTxt").GetComponent<TMP_Text>().text = roomPeopleNumValue;
            newItem.transform.Find("IsPrivateImg/IsPrivateTxt").GetComponent<TMP_Text>().text = privateValue;

            roomList.Add(newItem);
            closeMakeRoomPanel();
        }
    }

    void OnInputFieldValueChanged(TMP_InputField inputField, int maxLimit)
    {
        if (inputField.text.Length > maxLimit)
        {
            inputField.text = inputField.text.Substring(0, maxLimit);
        }
    }


    public void searchRoom()
    {
        string searchRoomName = roomSearchBar.text;
        foreach (GameObject room in roomList)
        {
            Debug.Log("aaa");
            string roomName = room.transform.Find("RoomNameTxt").GetComponent<TMP_Text>().text;
            if (searchRoomName.Equals(roomName))
            {
                room.SetActive(true);
            }
            else
            {
                room.SetActive(false);
            }
        }
    }
}
