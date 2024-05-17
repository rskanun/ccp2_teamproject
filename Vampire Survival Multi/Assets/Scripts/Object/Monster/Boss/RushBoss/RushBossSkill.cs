using DG.Tweening;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class RushBossSkill : MonoBehaviour
{
    [Header("돌진 설정")]
    [SerializeField] private float shortDistance;
    [SerializeField] private float shortChargingDelay;
    [SerializeField] private float longChargingDelay;
    [SerializeField] private float rushSpeed;
    [SerializeField] private float shortRushDamage;
    [SerializeField] private float longRushDamage;
    [SerializeField] private float stopDistance;

    [Header("사용 오브젝트")]
    [SerializeField] private GameObject rushRoute;
    [SerializeField] private GameObject pivot;
    [SerializeField] private SpriteRenderer sprite;

    [Header("참조 오브젝트")]
    [SerializeField] private RushBoss monster;

    // 현재 상테
    private bool isRushing = false;
    private bool isLongRush = false;
    private Vector2 targetPos;

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            // 호스트만 실행
            return;
        }

        if (isRushing)
        {
            OnRushAttack();
            OnMoveRush();
        }
    }

    private void OnRushAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, monster.Stat.AttackDistance);

        // 돌진 도중 주변 플레이어 데미지
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                Player player = collider.GetComponent<Player>();

                float damage = isLongRush ? longRushDamage : shortRushDamage;
                player.OnTakeDamage(damage);
            }
        }
    }

    private void OnMoveRush()
    {
        float speed = rushSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed);

        float distance = (targetPos - (Vector2)transform.position).magnitude;
        if (distance <= stopDistance)
        {
            isRushing = false;
            transform.position = targetPos;

            Invoke("OnArrive", 1f);
        }
    }

    private void OnArrive()
    {
        monster.OnRushStop();
    }

    public void OnRush(Vector3 targetPos)
    {
        this.targetPos = targetPos;

        StartCoroutine(RushToTarget(targetPos));
    }

    private IEnumerator RushToTarget(Vector3 targetPos)
    {

        float distance = (targetPos - transform.position).magnitude;

        isLongRush = distance > shortDistance;
        float delay = isLongRush ? longChargingDelay : shortChargingDelay;

        // 돌진 상태로 전환
        monster.OnRushing();

        // 돌진 전 차징
        ActiveRoute(transform.position, targetPos, monster.Stat.AttackDistance, delay);
        yield return new WaitForSeconds(delay);

        // 목표지점까지 돌진
        isRushing = true;
    }

    private void ActiveRoute(Vector2 bossPos, Vector2 targetPos, float attackDistance, float chargingDelay)
    {
        Vector2 spriteSize = sprite.bounds.size;

        // 경로 길이 설정
        float distance = Vector2.Distance(bossPos, targetPos);

        // 각도 조절
        Vector2 direction = (targetPos - bossPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 경로 애니메이션
        DOTween.Sequence()
            .OnStart(() =>
            {
                rushRoute.SetActive(true);
                rushRoute.transform.localPosition = new Vector3(-attackDistance / spriteSize.x, 0.0f);
                rushRoute.transform.localScale = new Vector3(0.0f, 2 * attackDistance / spriteSize.y);
            })
            .Append(rushRoute.transform.DOScaleX((distance + 2 * attackDistance) / spriteSize.x, chargingDelay))
            .OnComplete(() =>
            {
                rushRoute.SetActive(false);
            });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shortDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPos, monster.Stat.AttackDistance);
    }
}