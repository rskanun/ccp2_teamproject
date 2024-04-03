using TMPro;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    private class StatData
    {
        public float hp;
        public float str;
        public float def;
        public float agi;
    }

    // 임시 스텟 UI
    [SerializeField] private TextMeshProUGUI testStatInfo;

    [Header("참조 스크립트")]
    [SerializeField] private HealthUI healthUI;

    // 플레이어 스텟
    private LocalPlayerData playerStat;

    private void Start()
    {
        playerStat = LocalPlayerData.Instance;

        InitStat();
        InitHP(); // 초기 HP값 설정
    }

    /***************************************************************
     * [ 체력 설정 ]
     * 
     * 초기 체력 설정 및 체력 변화에 따른 스텟 변경
     ***************************************************************/

    private void InitHP()
    {
        playerStat.HP = playerStat.MaxHP;
    }

    public void UpdateHP()
    {
        float currentHP = playerStat.HP;
        float maxHP = playerStat.MaxHP;

        healthUI.UpdateHP(currentHP, maxHP);
    }

    /***************************************************************
     * [ 스테이터스 설정 ]
     * 
     * 플레이어의 스테이터스(체력, 근력, 민첩, 방어력) 설정
     ***************************************************************/

    private void InitStat()
    {
        // 기초 스텟 가져오기
        StatData stat = GetStat();

        // 스텟 적용
        SetStat(stat);
    }

    public void UpdateStat()
    {
        // 기초 스텟 가져오기
        StatData stat = GetStat();

        // 적용 스텟 계산
        foreach (ItemData item in PlayerEquip.Instance.EquipItems)
        {
            stat.hp += item.IncreasedHP;
            stat.str += item.IncreasedSTR;
            stat.def += item.IncreasedDEF;
            stat.agi += item.IncreasedAGI;
        }

        // 플레이어 스텟에 적용
        SetStat(stat);
    }

    private StatData GetStat()
    {
        ClassData classData = playerStat.Class;

        // 기초 스텟 가져오기
        StatData stat = new StatData();

        stat.hp = classData.HP;
        stat.str = classData.STR;
        stat.def = classData.DEF;
        stat.agi = classData.AGI;

        return stat;
    }

    private void SetStat(StatData stat)
    {
        playerStat.MaxHP = stat.hp;
        playerStat.STR = stat.str;
        playerStat.DEF = stat.def;
        playerStat.AGI = stat.agi;

        testStatInfo.text =
            "<Status>" +
            "\r\nSTR : " + stat.str +
            "\r\nDEF : " + stat.def +
            "\r\nAGI : " + stat.agi;
    }
}