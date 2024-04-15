using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 targetPos;
    private float damageRate;
    private float speed;

    public void CastProjectile(Vector2 targetPos, float speed, float damageRate)
    {
        this.targetPos = targetPos;
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

            OnDamage(monster);
        }
    }

    private void OnDamage(Monster monster)
    {
        PlayerData stat = LocalPlayerData.Instance.PlayerData;
        float damage = stat.STR * damageRate;

        monster.OnTakeDamage(damage);
    }
}