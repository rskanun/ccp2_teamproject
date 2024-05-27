using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/WandClass/NormalAttack", fileName = "Dark Matter")]
public class DarkMatter : Skill
{
    // 스킬 세부 사항
    [SerializeField] private float distance;
    [SerializeField] private float speed;

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

        // 피격 데미지
        float damage = caster.PlayerData.STR * 0.75f + caster.PlayerData.DEF * 0.5f;

        // 투척 각도
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 투사체 발사
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.ThrowProjectile(targetPos, speed, angle, false, (monster) =>
        {
            caster.OnNormalAttack(monster, damage);
        });
    }
}