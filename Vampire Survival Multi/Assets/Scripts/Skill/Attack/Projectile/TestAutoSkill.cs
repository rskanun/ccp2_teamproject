using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Skill/Class/Auto/Test", fileName = "Test Auto Skill")]
public class TestAutoSkill : Skill
{
    // 스킬 세부 사항
    [SerializeField] private bool isPiercing;
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

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetPos = casterPos + (mousePos - casterPos).normalized * distance;


        // 투사체 발사 방향으로 회전
        float angle = Mathf.Atan2((mousePos - casterPos).normalized.y, (mousePos - casterPos).normalized.x) * Mathf.Rad2Deg;
        projectileObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        // 투사체 발사
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.ThrowProjectile(targetPos, speed, isPiercing, (monster) =>
        {
            caster.OnNormalAttack(monster, caster.PlayerData.STR);
        });
    }
}