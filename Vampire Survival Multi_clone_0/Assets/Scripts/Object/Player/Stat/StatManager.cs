using TMPro;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    // 임시 스텟 UI
    [SerializeField] private TextMeshProUGUI testStatInfo;

    // 플레이어 스텟
    private PlayerData playerStat;

    private void Start()
    {
        playerStat = LocalPlayerData.Instance.PlayerData;

        // 임시 UI 업데이트
        UpdateTmpUI();
    }

    /***************************************************************
     * [ 스테이터스 설정 ]
     * 
     * 상황에 따른 플레이어의 스테이터스(체력, 근력, 민첩, 방어력) 설정
     ***************************************************************/

    public void EquipItem()
    {
        // 적용 스텟 계산
        ItemData item = PlayerEquip.Instance.LastItem;

        playerStat.MaxHP = GetEquipHP(item);
        playerStat.STR = GetEquipSTR(item);
        playerStat.DEF = GetEquipDEF(item);
        playerStat.AttackSpeed = GetEquipAttackSpeed(item);
        playerStat.MoveSpeed = GetEquipMoveSpeed(item);
        playerStat.LifeSteal = GetEquipLifeSteal(item);

        // 임시 UI 업데이트
        UpdateTmpUI();
    }

    private float GetEquipHP(ItemData item)
    {
        float hp = playerStat.MaxHP;

        hp = hp + item.HP;
        hp = hp + (hp * item.PercentHP);

        return hp;
    }

    private float GetEquipSTR(ItemData item)
    {
        float str = playerStat.STR;

        str = str + item.STR;
        str = str + (str * item.PercentSTR);

        return str;
    }

    private float GetEquipDEF(ItemData item)
    {
        float def = playerStat.DEF;

        def = def + item.DEF;
        def = def + (def * item.PercentDEF);

        return def;
    }

    private float GetEquipAttackSpeed(ItemData item)
    {
        float speed = 100.0f / (100 + item.AttackSpeed) * playerStat.AttackSpeed;

        return speed;
    }

    private float GetEquipMoveSpeed(ItemData item)
    {
        ClassData classData = LocalPlayerData.Instance.Class;

        float originSpeed = classData.MoveSpeed;
        float speed = playerStat.MoveSpeed + (originSpeed * item.MoveSpeed);

        return speed;
    }

    private float GetEquipLifeSteal(ItemData item)
    {
        float lifeSteal = playerStat.LifeSteal + item.ListSteal;

        return lifeSteal;
    }

    private void UpdateTmpUI()
    {
        testStatInfo.text =
            "<Status>" +
            "\r\nHP : " + playerStat.MaxHP +
            "\r\nSTR : " + playerStat.STR +
            "\r\nDEF : " + playerStat.DEF +
            "\r\nSpeed : " + playerStat.MoveSpeed +
            "\r\nAttack Speed : " + playerStat.AttackSpeed +
            "\r\nLife Steal : " + playerStat.LifeSteal;
    }
}