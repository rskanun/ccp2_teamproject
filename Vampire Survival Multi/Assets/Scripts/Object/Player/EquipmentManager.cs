using TMPro;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private class StatData
    {
        public float hp;
        public float str;
        public float def;
        public float agi;
    }

    [SerializeField]
    private TextMeshProUGUI testStatInfo;

    public void UpdateStat()
    {
        ClassData classData = PlayerStatus.Instance.Class;

        // 기초 스텟 가져오기
        StatData stat = new StatData();

        stat.hp = classData.HP;
        stat.str = classData.STR;
        stat.def = classData.DEF;
        stat.agi = classData.AGI;

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

    private void SetStat(StatData stat)
    {
        PlayerStatus playerStat = PlayerStatus.Instance;

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