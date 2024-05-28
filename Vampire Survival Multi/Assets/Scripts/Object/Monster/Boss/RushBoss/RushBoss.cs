using UnityEngine;

public class RushBoss : BossMonster
{
    [Header("보스 공격 데이터")]
    [SerializeField] private float rushCooldown;
    [SerializeField] private RushBossSkill rush;

    private float cooldown;

    // 보스 스킬 상태
    private bool isRushing;

    protected override void OnCastSkill()
    {
        if (isRushing == false && cooldown <= 0.0f)
        {
            // 가장 가까운 플레이어를 타겟으로 돌진
            GameObject target = GetTarget();
            rush.OnRush(target.transform.position);
        }
    }

    private GameObject GetTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 가장 가까운 플레이어 위치 리턴
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    public void OnRushing()
    {
        isRushing = true;
    }

    public void OnRushStop()
    {
        // 쿨타임 설정
        cooldown = rushCooldown;

        isRushing = false;
    }

    protected override void OnCooldown()
    {
        if (cooldown > 0.0f)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public override void OnMove(Vector2 targetPos)
    {
        if (!isRushing)
        {
            // 대쉬 중이 아닐 경우에만 움직임
            base.OnMove(targetPos);
        }
    }
}