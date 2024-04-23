/*
 using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MultiGameController : MonoBehaviour
{
    public GameObject multiPanel;           //멀티 게임 패널
    public GameObject makeRoomPanel;        //방 만들기 패널
    public GameObject askEnterPanel;        //방 진입 확인 패널

    public Button[] btnsActiveByMakeRoom;       //방 만들기 패널 여닫기에 따라 활성화가 바뀌는 버튼들
    public Button[] btnsActiveByAskEnter;       //방 진입 확인 패널 여닫기에 따라 활성화가 바뀌는 버튼들

    //멀티 게임 패널에 존재하는 Object들
    public TMP_InputField roomSearchBar;        //빙 검색 바
    public ScrollRect scrollView;               //방 목록 나오는 스크롤
    public Button searchByNameBtn;              //이름으로 방 찾기 버튼
    public Button searchByCodeBtn;              //코드로 방 찾기 버튼
    public TextMeshProUGUI placeHolder;        //placeHolder

    //방 진입 확인 패널에 존재하는 Object들
    public TMP_Dropdown roomPeopleNumDropdown;  //방 인원수 설정 드롭다운
    public TMP_Dropdown privateDropdown;        //방 공개여부 드롭다운
    public TMP_InputField passwordInputField;   //방 비밀번호 입력바
    public TMP_InputField roomNameInputField;   //방 이름 입력바 

    public int maxPasswordLimit = 8;            //패스워드 수 제한
    public int maxRoomNameLimit = 12;           //방 이름 글자수 제한

    private string initialRoomPeopleNumValue;       //기본 방 인원수(2명)
    private string initialPrivateValue;             //기본 방 공개여부(공개)
    private string initialPassword;                 //기본 방 비밀번호("")
    private string initialRoomName;                 //기본 방 이름("")

    //프리팹
    public GameObject roomPrefab;               //방 정보 및 입력 프리팹

    //RoomData 리스트(생성된 방들 리스트)
    [SerializeField]
    private List<RoomData> roomDatas;

    //열거형 방 타입(멀티 패널 오픈시 바로 리스트에 나오는 방들)
    public enum RoomType { Room1, Room2 };

    void Start()
    {
        for (int i = 0; i < roomDatas.Count; i++)
        {
            var room = makeRoom((RoomType)i);
        }

        initialRoomPeopleNumValue = roomPeopleNumDropdown.options[roomPeopleNumDropdown.value].text;
        initialPrivateValue = privateDropdown.options[privateDropdown.value].text;
        initialPassword = passwordInputField.text;
        initialRoomName = roomNameInputField.text;

        roomNameInputField.characterLimit = maxRoomNameLimit;
        roomNameInputField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(roomNameInputField, maxRoomNameLimit); });

        passwordInputField.characterLimit = maxPasswordLimit;
        passwordInputField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(passwordInputField, maxPasswordLimit); });
    }

    void Update()
    {
        enableMakePassword();
        setSearchBarState();
    }

    //방 만들기 패널 열기 + 버튼 비활성화
    public void openMakeRoomPanel() 
    {
        makeRoomPanel.SetActive(true);
        foreach (Button btns in btnsActiveByMakeRoom)
        {
            btns.interactable = false;
        }
        roomSearchBar.interactable = false;
    }

    //방 진입 확인 패널 열기 + 버튼 비활성화 (RoomInfo프리팹 기능)xxxx
    public void openAskEnterPanel() 
    {
        askEnterPanel.SetActive(true);
        foreach (Button btns in btnsActiveByAskEnter)
        {
            btns.interactable = false;
        }
    }

    //멀티 게임 패널 닫기
    public void closeMultiPanel() 
    {
        multiPanel.SetActive(false);
    }

    //방 만들기 패널 닫기 + 버튼 활성화 + 작성한 방 설정들 초기화
    public void closeMakeRoomPanel() 
    {
        makeRoomPanel.SetActive(false);
        foreach (Button btns in btnsActiveByMakeRoom)
        {
            btns.interactable = true;
        }
        roomSearchBar.interactable = true;

        roomPeopleNumDropdown.value = roomPeopleNumDropdown.options.FindIndex(option => option.text == initialRoomPeopleNumValue);
        privateDropdown.value = privateDropdown.options.FindIndex(option => option.text == initialPrivateValue);
        passwordInputField.text = initialPassword;
        roomNameInputField.text = initialRoomName;
    }

    //빙 검색 바 택스트 삭제
    public void deleteTxt()
    {
        roomSearchBar.text = "";
    }

    //inputField 안의 글자수 제한
    void OnInputFieldValueChanged(TMP_InputField inputField, int maxLimit)
    {
        if (inputField.text.Length > maxLimit)
        {
            inputField.text = inputField.text.Substring(0, maxLimit);
        }
    }

    //리스트에 방 추가 및 방 생성+ 동시에 패널 닫기
    public void addRoomToList()
    {
        string roomPeopleNumValue = roomPeopleNumDropdown.options[roomPeopleNumDropdown.value].text;
        string privateValue = privateDropdown.options[privateDropdown.value].text;
        string password = passwordInputField.text;
        string roomName = roomNameInputField.text;
        string roomCode = Guid.NewGuid().ToString("N").Substring(0, 12);

        if (roomName.Equals("") || (privateValue.Equals("비공개") && password.Equals("")))
        {
            Debug.Log("필수입력란 누락!");
        }
        else
        {
            int maxPeopleNum = int.Parse(roomPeopleNumValue);
            bool isPrivate = privateValue.Equals("비공개");

            RoomData newRoomData = new RoomData(roomName, password, maxPeopleNum, privateValue.Equals("비공개"),roomCode);
            roomDatas.Add(newRoomData);

            // 방을 열거형 RoomType에 추가
            RoomType roomType = (RoomType)roomDatas.Count - 1;
            makeRoom(roomType);
            closeMakeRoomPanel();
        }
    }

    //열거형 roomType에 있던 방들을 리스트에 보이게 하여 방을 생성
    public Room2 makeRoom(RoomType type)
    {
        var newRoom = Instantiate(roomPrefab, scrollView.content).GetComponent<Room2>();
        newRoom.roomData = roomDatas[(int)type];
        newRoom.name = newRoom.roomData.RoomName;
        newRoom.transform.Find("RoomNameTxt").GetComponent<TMP_Text>().text = newRoom.roomData.RoomName;
        newRoom.transform.Find("PasswordTxt").GetComponent<TMP_Text>().text = newRoom.roomData.Password;
        newRoom.transform.Find("RoomCodeTxt").GetComponent<TMP_Text>().text = newRoom.roomData.RoomCode;
        newRoom.transform.Find("RoomPeopleNumImg/RoomPeopleNumTxt").GetComponent<TMP_Text>().text = newRoom.roomData.MaxPeopleNum.ToString() + "명";
        if (newRoom.roomData.IsPrivate)
        {
            newRoom.transform.Find("IsPrivateImg/IsPrivateTxt").GetComponent<TMP_Text>().text = "비공개";
        }
        else
        {
            newRoom.transform.Find("IsPrivateImg/IsPrivateTxt").GetComponent<TMP_Text>().text = "공개";
        }

        return newRoom;
    }

    //비공개 시 비밀번호 생성 활성화, 공개시 비밀번호 생성 비활성화(방 만들기)
    public void enableMakePassword()        
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

    //방 검색 - 이름 검색과 코드 검색으로 나누기
    public void searchRoom()
    {
        string searchText = roomSearchBar.text;
        RectTransform content = scrollView.content;

        if(placeHolder.text == "방 이름 검색...")
        {
            foreach (Transform room in content)
            {
                // 각 child gameobject의 이름을 가져옵니다.
                string roomName = room.Find("RoomNameTxt").GetComponent<TMP_Text>().text;
                room.gameObject.SetActive(searchText.Equals(roomName));
            }
        }
        else if (placeHolder.text == "방 코드 검색...")
        {
            foreach (Transform room in content)
            {
                // 각 child gameobject의 이름을 가져옵니다.
                string roomCode = room.Find("RoomCodeTxt").GetComponent<TMP_Text>().text;
                room.gameObject.SetActive(searchText.Equals(roomCode));
            }
        }
        else
        {
            Debug.Log("검색 오류");
        }

    }

    //방 이름으로 검색 / placeholder 변경 / 이름 버튼 비활성화, 코드 버튼 활성화
    public void changeSearchTypeName()
    {
        //searchByNameBtn.interactable = false;
        //searchByCodeBtn.interactable = true;
        placeHolder.text = "방 이름 검색...";
    }

    //방 이름으로 검색 / placeholder 변경 / 이름 버튼 비활성화, 코드 버튼 활성화
    public void changeSearchTypeCode()
    {
        //searchByNameBtn.interactable = true;
        //searchByCodeBtn.interactable = false;
        placeHolder.text = "방 코드 검색...";
    }

    //방 검색바 상태 변경
    private void setSearchBarState()
    {
        if(makeRoomPanel.activeSelf)
        {
            roomSearchBar.interactable = false;
        }
        else
        {
            roomSearchBar.interactable = true;
        }
    }

}

 */