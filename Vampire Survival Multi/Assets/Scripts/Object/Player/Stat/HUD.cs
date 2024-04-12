using UnityEngine;

public class HUD : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private HealthUI healthUI;

    // 참조 데이터
    private PlayerData stat;

    private void Start()
    {
        stat = LocalPlayerData.Instance.PlayerData;

        // Init HUD
        UpdateHP();
    }

    public void UpdateHP()
    {
        float currentHP = stat.HP;
        float maxHP = stat.MaxHP;

        healthUI.UpdateHP(currentHP, maxHP);
    }
}