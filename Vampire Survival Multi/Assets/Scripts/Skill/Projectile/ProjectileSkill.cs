using UnityEngine;

[CreateAssetMenu(menuName = "Skill/NonTarget/Projectile", fileName = "Projectile SKill")]
public class ProjectileSkill : Skill
{
    [Header("발사체")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("스킬 정보")]
    [SerializeField] private float damageRate;
    [SerializeField] private float distance;
    [SerializeField] private float speed;

    protected override void CastSkill()
    {
        GameObject projectileObj = Instantiate(projectilePrefab);

        // 투사체 생성 위치 설정
        PlayerData playerData = LocalPlayerData.Instance.PlayerData;
        projectileObj.transform.position = playerData.Position;

        // 이동 방향 설정
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 casterPos = playerData.Position;
        Vector2 targetPos = casterPos + (mousePos - casterPos).normalized * distance;

        // 투사체 발사
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.CastProjectile(targetPos, speed, damageRate);
    }
}