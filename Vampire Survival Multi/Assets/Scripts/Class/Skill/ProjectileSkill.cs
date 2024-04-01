using UnityEngine;

[CreateAssetMenu(menuName = "Skill/NonTarget/Projectile", fileName = "Projectile SKill")]
public class ProjectileSkill : Skill
{
    [Header("스킬 정보")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float distance;
    [SerializeField] private float speed;

    protected override void CastSkill()
    {
        GameObject projectileObj = Instantiate(projectilePrefab);

        // 투사체 생성 위치 설정
        projectileObj.transform.position = PlayerStatus.Instance.Position;

        // 이동 방향 설정
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 casterPos = PlayerStatus.Instance.Position;
        Vector2 targetPos = casterPos + (mousePos - casterPos).normalized * distance;

        // 투사체 발사
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.CastProjectile(targetPos, speed);
    }
}