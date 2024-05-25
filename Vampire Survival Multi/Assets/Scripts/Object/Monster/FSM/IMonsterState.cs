public interface IMonsterState
{
    public void OnEnterState(FSM fsm);
    public void OnUpdate(FSM fsm);
    public void OnExitState(FSM fsm);
}