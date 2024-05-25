using UnityEngine;

public class ChaseState : IMonsterState
{
    private Monster monster;

    public ChaseState(Monster monster)
    {
        this.monster = monster;
    }

    public void OnUpdate(FSM fsm)
    {
        GameObject target = DetectedPlayer();
        
        if (target != null)
        {
            Vector2 pos = target.transform.position;

            // 몬스터와 플레이어 간의 거리 계산
            float distance = Vector2.Distance(pos, monster.transform.position);
            if (distance <= monster.Stat.AttackDistance)
            {
                // 몬스터가 공격 가능한 범위까지 접근하면 상태 변경
                fsm.SetState(new AttackState(monster));
            }
            else
            {
                // 특정 플레이어를 향해 이동
                monster.OnMove(target.transform.position);
            }
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
            float distance = Vector2.Distance(player.transform.position, monster.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    public void OnEnterState(FSM fsm) { }

    public void OnExitState(FSM fsm) { }
}