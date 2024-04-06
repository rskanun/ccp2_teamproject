using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 targetPos;
    private float speed;

    public void CastProjectile(Vector2 targetPos, float speed)
    {
        this.targetPos = targetPos;
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
        float damage = LocalPlayerData.Instance.STR;

        monster.OnTakeDamage(damage);
    }
}