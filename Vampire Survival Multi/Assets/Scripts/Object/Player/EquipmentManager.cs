using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("장비 중인 아이템 리스트")]
    [SerializeField] private List<ItemData> equipItems;

    public void EquipItem(ItemData item)
    {
        // 장비 장착
        equipItems.Add(item);

        // 장비 장착 후 플레이어 스텟 변경
        UpdateStat();
    }

    private void UpdateStat()
    {
        PlayerStat stat = PlayerStat.Instance;

        // 기초 스텟 가져오기
        int hp = stat.HP;
        int str = stat.STR;
        int def = stat.DEF;
        int agi = stat.AGI;

        // 적용 스텟 계산
        foreach (ItemData item in equipItems)
        {
            hp += item.IncreasedHP;
            str += item.IncreasedSTR;
            def += item.IncreasedDEF;
            agi += item.IncreasedDEF;
        }

        // 플레이어 스텟에 적용
        stat.HP = hp;
        stat.STR = str;
        stat.DEF = def;
        stat.AGI = agi;
    }
}