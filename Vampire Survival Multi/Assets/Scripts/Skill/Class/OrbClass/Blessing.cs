using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/OrbClass/ActiveSkill", fileName = "Blessing")]
public class Blessing : Skill
{
    [Header("범위 오브젝트")]
    [SerializeField] private GameObject skillAreaPrefab;

    // 플레이어별 스킬 상태
    private Dictionary<int, OrbSkillArea> skillDictionary = new Dictionary<int, OrbSkillArea>();

    public override void InitSkill(Player caster)
    {
        // 플레이어에게 영역 생성
        string name = SKILL_OBJECT_DIRECTORY + skillAreaPrefab.name;
        Vector2 casterPos = caster.PlayerData.Position;
        GameObject area = PhotonNetwork.Instantiate(name, casterPos, Quaternion.identity);

        // 스킬 초기 설정
        OrbSkillArea skillArea = area.GetComponent<OrbSkillArea>();
        skillArea.SetCaster(caster);
        skillArea.SetBuffType(true);

        // 위치 설정
        skillArea.InitParent(caster.photonView.ViewID);

        // 플레이어별 스킬 설정
        skillDictionary[caster.photonView.ViewID] = skillArea;
    }

    public override void UseSkill(Player caster, Vector2 direction)
    {
        int id = caster.photonView.ViewID;

        if (skillDictionary.ContainsKey(id))
        {
            skillDictionary[id].OnSwitchBuff();
        }
    }
}