using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private Player player;

    void Start()
    {
        player.InitStat();
    }
}