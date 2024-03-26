using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("초기 클래스")]
    [SerializeField] private ClassData classData;

    [Header("참조 스크립트")]
    [SerializeField] private PlayerTracker trakerCamera;

    [Header("사용 오브젝트")]
    [SerializeField] private GameObject player;

    void Start()
    {
        // 초기 클래스 설정
        PlayerStatus.Instance.Class = classData;

        // 장비 초기 셋팅
        PlayerEquip.Instance.InitEquips();

        GameStart();
    }

    private void GameStart()
    {
        GameObject PlayerObj = Instantiate(player);

        trakerCamera.InitPlayer(PlayerObj);
    }
}