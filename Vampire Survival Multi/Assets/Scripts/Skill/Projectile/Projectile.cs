using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 targetPos;
    private Player caster;
    private float damageRate;
    private float speed;

    public void CastProjectile(Vector2 targetPos, Player caster, float speed, float damageRate)
    {
        this.targetPos = targetPos;
        this.caster = caster;
        this.damageRate = damageRate;
        this.speed = speed;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if ((Vector2)transform.position == targetPos)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            // 닿은 오브젝트가 몬스터이면 데미지
            Monster monster = collision.GetComponent<Monster>();

            caster.OnAttack(monster, damageRate);
        }
    }
}