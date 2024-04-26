using UnityEngine;

public class Projectile : MonoBehaviour
{
    private delegate void OnHit(Monster monster);
    private delegate void OnMove();
    private event OnHit onHitEvent;
    private event OnMove onMoveEvent;

    public void ThrowProjectile(Vector2 targetPos, Player attacker, float speed, float damage, bool isPiercing)
    {
        onMoveEvent = () => MoveTo(targetPos, speed);
        onHitEvent = (monster) =>
        {
            attacker.OnAttack(monster, damage);

            // 관통이 아닐 경우 파괴
            if (isPiercing == false) Destroy(gameObject);
        };
    }

    private void FixedUpdate()
    {
        onMoveEvent?.Invoke();
    }

    private void MoveTo(Vector2 targetPos, float speed)
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
            Monster monster = collision.GetComponent<Monster>();

            onHitEvent?.Invoke(monster);
        }
    }
}