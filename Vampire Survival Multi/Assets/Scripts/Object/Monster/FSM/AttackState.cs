using UnityEngine;

public class AttackState : IMonsterState
{
    private Monster monster;
    private GameObject target;

    public AttackState(Monster monster)
    {
        this.monster = monster;
    }

    public void OnEnterState(FSM fsm)
    {
        GameObject target = DetectedPlayer();

        if (target != null)
        {
            this.target = target;
        }
        else
        {
            // 플레이어를 찾지 못했으면 초기 상태로 전환
            fsm.SetState(new ChaseState(monster));
        }
    }

    private GameObject DetectedPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 가장 가까운 플레이어 위치 리턴
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            Player p = player.GetComponent<Player>();

            // 은신 상태 플레이어 제외
            if (p.HasBuff(Buff.Invisible) == false)
            {
                float distance = Vector2.Distance(player.transform.position, monster.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }
        }

        return closestPlayer;
    }

    public void OnUpdate(FSM fsm)
    {
        // 선 공격
        OnAttack(target);

        // 해당 타겟이 여전히 공격 가능한 범위 내에 있는지 판별
        if (target.activeSelf == true)
        {
            Vector2 mobPos = monster.transform.position;
            Vector2 playerPos = target.transform.position;

            // 몬스터와 플레이어 간의 거리 계산
            float distance = Vector2.Distance(playerPos, mobPos);
            if (distance > monster.Stat.AttackDistance)
            {
                // 거리가 멀어지면 다시 추적
                fsm.SetState(new ChaseState(monster));
            }
        }
        else
        {
            // 해당 타겟이 죽은 상태면 추적 상태로 전환
            fsm.SetState(new ChaseState(monster));
        }
    }

    private void OnAttack(GameObject target)
    {
        // 플레이어 공격
        monster.OnAttack(target);
    }

    public void OnExitState(FSM fsm) { }
}