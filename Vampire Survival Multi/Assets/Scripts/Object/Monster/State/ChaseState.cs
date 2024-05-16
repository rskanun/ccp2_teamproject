using System;
using UnityEngine;

public class ChaseState : IMonsterState
{
<<<<<<< Updated upstream
    private Monster monster;

    public ChaseState(Monster monster)
    {
        this.monster = monster;
=======
    private Monster _monster;
    private SpriteRenderer spriter;

    public ChaseState(Monster monster)
    {
        _monster = monster;
        spriter = _monster.GetComponent<SpriteRenderer>();
>>>>>>> Stashed changes
    }

    public void OnUpdate(FSM fsm)
    {
        GameObject target = DetectedPlayer();
        
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
        
=======
>>>>>>> 3dafc852bf63d9812eb4e4d163bb0288b895f612
>>>>>>> Stashed changes
        if (target != null)
        {
            Vector2 pos = target.transform.position;

            // 몬스터와 플레이어 간의 거리 계산
<<<<<<< Updated upstream
            float distance = Vector2.Distance(pos, monster.transform.position);
            if (distance <= monster.MonsterData.AttackDistance)
            {
                // 몬스터가 공격 가능한 범위까지 접근하면 상태 변경
                fsm.SetState(new AttackState(monster));
=======
            float distance = Vector2.Distance(pos, _monster.transform.position);
            if (distance <= _monster.MonsterData.AttackDistance)
            {
                // 몬스터가 공격 가능한 범위까지 접근하면 공격
                fsm.SetState(new AttackState(_monster, target));
>>>>>>> Stashed changes
            }
            else
            {
                // 특정 플레이어를 향해 이동
<<<<<<< Updated upstream
                monster.OnMove(target.transform.position);
=======
                MoveToPlayer(pos);
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    public void OnEnterState(FSM fsm) { }
=======
    private void MoveToPlayer(Vector2 playerPos)
    {
        float speed = _monster.MonsterData.MoveSpeed * Time.deltaTime;

        _monster.transform.position = Vector2.MoveTowards(_monster.transform.position, playerPos, speed);
        spriter.flipX = _monster.transform.position.x < 0;
    }

    public void OnEnterState() { }
>>>>>>> Stashed changes

    public void OnExitState(FSM fsm) { }
}