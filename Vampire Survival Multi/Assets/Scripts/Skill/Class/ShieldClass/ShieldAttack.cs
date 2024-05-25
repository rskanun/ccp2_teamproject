using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/ShieldClass/NormalAttack", fileName = "Shield Swing")]
public class ShieldAttack : Skill
{
    [Header("타격 오브젝트")]
    [SerializeField] private GameObject swingPrefab;

    public override void UseSkill(Player caster, Vector2 direction)
    {
        PlayerData casterData = caster.PlayerData;
        Vector2 casterPos = casterData.Position;

        string prefabName = SKILL_OBJECT_DIRECTORY + swingPrefab.name;
        GameObject obj = PhotonNetwork.Instantiate(prefabName, casterPos, Quaternion.identity);

        // 스킬 오브젝트 생성 위치 설정
        obj.transform.position = casterPos;

        // 타격 방향 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        SwingObject swingObj = obj.GetComponent<SwingObject>();

        swingObj.InitParent(caster.photonView.ViewID);
        swingObj.OnSwing(angle, casterData.AttackSpeed, false, (monster) =>
        {
            float damage = casterData.DEF * 0.75f + casterData.STR * 0.25f;

            caster.OnNormalAttack(monster, damage);
        });
    }
}