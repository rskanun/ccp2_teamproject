using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/WandClass/ActiveSkill", fileName = "Black Death")]
public class BlackDeath : Skill
{
    // 스킬 세부 사항
    [SerializeField] private float distance;
    [SerializeField] private float speed;
    [SerializeField] private float effectRadius;
    [SerializeField] private float effectDuration;

    [Header("생성 오브젝트")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject areaPrefab;

    public override void UseSkill(Player caster, Vector2 direction)
    {
        Vector2 casterPos = caster.PlayerData.Position;

        string prefabName = SKILL_OBJECT_DIRECTORY + projectilePrefab.name;
        GameObject projectileObj = PhotonNetwork.Instantiate(prefabName, casterPos, Quaternion.identity);

        // 투사체 생성 위치 설정
        projectileObj.transform.position = casterPos;

        // 이동 방향 설정
        Vector2 targetPos = casterPos + direction * distance;

        // 투척 각도
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 투사체 발사
        WandSkillProjectile projectile = projectileObj.GetComponent<WandSkillProjectile>();

        projectile.ThrowProjectile(targetPos, speed, angle, effectRadius, effectDuration, SKILL_OBJECT_DIRECTORY + areaPrefab.name);
    }
}