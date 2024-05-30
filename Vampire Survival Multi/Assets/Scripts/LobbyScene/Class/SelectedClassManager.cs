using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SelectedClassUI))]
public class SelectedClassManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SelectedClassUI ui;
    [SerializeField] private LobbyManager lobbyManager;

    public void Start()
    {
        // ClassList가 null인지, 요소가 있는지 확인
        if (ClassResource.Instance.ClassList == null || ClassResource.Instance.ClassList.Count == 0)
        {
            Debug.LogError("ClassList is either null or empty");
            return;
        }

        // 초기 클래스 설정
        ClassData initClass = ClassResource.Instance.ClassList[0];

        UpdateSelectClass(initClass);
    }

    public void UpdateSelectClass(ClassData classData)
    {
        // 로컬 플레이어의 직업 설정
        LocalPlayerData.Instance.SetClass(classData);

        ui.SetClassInfo(classData);
    }
}