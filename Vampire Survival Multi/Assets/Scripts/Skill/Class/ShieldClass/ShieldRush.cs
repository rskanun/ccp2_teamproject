using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/ShieldClass/ActiveSkill", fileName = "Shield Rush")]
public class ShieldRush : Skill
{
    // 스킬 세부 사항
    [SerializeField] private Vector2 knockbackArea;
    [SerializeField] private float knockbackPower;
    [SerializeField] private float stunDuration;

    private Vector2 point;

    public override void UseSkill(Player caster, Vector2 direction)
    {
        Debug.Log("shield rush");
        // 돌진 범위 설정
        Vector2 point = direction * (knockbackArea.x / 2.0f);
        this.point = point;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(point, knockbackArea, angle);
        foreach(Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                Vector2 force = direction * knockbackPower;
                Rigidbody2D rigid = collider.GetComponent<Rigidbody2D>();
                Monster monster = rigid.GetComponent<Monster>();

                // 범위 안 몬스터 밀치기 + 데미지 + 상태이상 스턴
                OnKnockback(rigid, force);
                OnTakeDamage(monster, caster);
                AddEffect(monster);
            }
        }

        // 플레이어 이동
        Vector2 rushPos = (Vector2)caster.transform.position + direction * knockbackArea.x;
        caster.transform.position = rushPos;
    }

    public void OnDraw()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(point, knockbackArea);
    }

    private void OnKnockback(Rigidbody2D rigid, Vector2 force)
    {
        Debug.Log($"{rigid.GetComponent<PhotonView>().ViewID} KNOCKBACK!");
        rigid.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnTakeDamage(Monster target, Player caster)
    {
        float damage = 10 + caster.PlayerData.DEF * 0.25f;

        target.OnTakeDamage(caster, damage);
    }

    private void AddEffect(Monster target)
    {
        target.AddEffect(StatusEffect.Stun, stunDuration);
    }
}