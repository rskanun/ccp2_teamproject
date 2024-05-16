using UnityEngine;

public class BossB : BossMonster
{
    [Header("보스 공격 데이터")]
    [SerializeField] private float rushCooldown;
    [SerializeField] private BossBSkill rush;

    // 보스 스킬 상태
    private bool isRushing;

    protected override void OnCastSkill()
    {
        if (CoolTime <= 0.0f)
        {
            // 가장 가까운 플레이어를 타겟으로 돌진
            GameObject target = GetTarget();
            rush.OnRush(target.transform.position);

            // 쿨타임 설정
            CoolTime = rushCooldown;
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
        // 상태 변경
        fsm.SetState(null);
    }

    public void OnRushStop()
    {
        // 상태 변경
        fsm.SetState(new ChaseState(this));
    }
}