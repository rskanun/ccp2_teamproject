using UnityEngine;

public class HUD : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private HealthUI healthUI;

    // 참조 데이터
    private PlayerData _stat;
    private PlayerData Stat
    {
        get
        {
            if (_stat == null)
                _stat = LocalPlayerData.Instance.PlayerData;

            return _stat;
        }
    }

    public void UpdateHP()
    {
        float currentHP = Stat.HP;
        float maxHP = Stat.MaxHP;

        healthUI.UpdateHP(currentHP, maxHP);
    }
}