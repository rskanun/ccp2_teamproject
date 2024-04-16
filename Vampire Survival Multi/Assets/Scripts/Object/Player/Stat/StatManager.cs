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

    // 플레이어 스텟
    private PlayerData playerStat;

    private void Start()
    {
        playerStat = LocalPlayerData.Instance.PlayerData;

        InitStat();
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
        stat.agi = classData.Speed;

        return stat;
    }

    private void ApplyStat(StatData stat)
    {

    }

    private void SetStat(StatData stat)
    {
        playerStat.MaxHP = stat.hp;
        playerStat.STR = stat.str;
        playerStat.DEF = stat.def;
        playerStat.Speed = stat.agi;

        testStatInfo.text =
            "<Status>" +
            "\r\nHP : " + stat.hp +
            "\r\nSTR : " + stat.str +
            "\r\nDEF : " + stat.def +
            "\r\nAGI : " + stat.agi;
    }
}