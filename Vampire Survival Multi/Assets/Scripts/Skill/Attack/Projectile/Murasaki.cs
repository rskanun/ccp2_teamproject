using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/Active/Test", fileName = "Murasaki")]
public class Murasaki : Skill
{
    // 스킬 세부 사항
    [SerializeField] private float distance;
    [SerializeField] private float speed;
    [SerializeField] private float damageValue;
    [SerializeField] private float damageRate;

    [Header("발사체")]
    [SerializeField] private GameObject projectilePrefab;

    public override void UseSkill(Player caster, Vector2 direction)
    {
        Vector2 casterPos = caster.PlayerData.Position;

        string prefabName = SKILL_OBJECT_DIRECTORY + projectilePrefab.name;
        GameObject projectileObj = PhotonNetwork.Instantiate(prefabName, casterPos, Quaternion.identity);

        // 투사체 생성 위치 설정
        projectileObj.transform.position = casterPos;

        // 이동 방향 설정
        Vector2 targetPos = casterPos + direction * distance;

        float damage = damageValue + caster.PlayerData.STR * damageRate;

        // 투사체 발사
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.ThrowProjectile(targetPos, speed, true, (monster) =>
        {
            caster.OnSkillAttack(monster, damage);
        });
    }
}