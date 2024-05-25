using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/AxeClass/NormalAttack", fileName = "Axe Swing")]
public class AxeAttack : Skill
{
    [Header("타격 오브젝트")]
    [SerializeField] private GameObject swingPrefab;

    public override void UseSkill(Player caster, Vector2 direction)
    {
        PlayerData casterData = caster.PlayerData;
        Vector2 casterPos = casterData.Position;

        string prefabName = SKILL_OBJECT_DIRECTORY + swingPrefab.name;
        GameObject obj = PhotonNetwork.Instantiate(prefabName, casterPos, Quaternion.identity);
        obj.transform.SetParent(caster.transform);

        // 스킬 오브젝트 생성 위치 설정
        obj.transform.position = casterPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        SwingObject swingObj = obj.GetComponent<SwingObject>();

        swingObj.OnSwing(angle, casterData.AttackSpeed, true, (monster) =>
        {
            float damage = casterData.STR * 0.75f + casterData.DEF * 0.25f;

            caster.OnNormalAttack(monster, damage);
        });
    }
}