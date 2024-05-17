using UnityEngine;

public class FSM
{
    private IMonsterState _curState;

    public FSM(IMonsterState initState)
    {
        _curState = initState;
    }

    public void SetState(IMonsterState state)
    {
        // 기존 상태 나가기
        _curState?.OnExitState(this);

        // 새 상태 설정
        _curState = state;
        _curState?.OnEnterState(this);
    }

    public void OnAction()
    {
        _curState?.OnUpdate(this);
    }
}