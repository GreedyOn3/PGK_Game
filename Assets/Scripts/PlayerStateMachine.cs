public class PlayerStateMachine
{
    PlayerState currState;

    public void Initialize(PlayerState startState)
    {
        currState = startState;
        currState.Enter();
    }

    public void ChangeState(PlayerState nextState)
    {
        currState.Exit();
        currState = nextState;
        currState.Enter();
    }

    public void Update()
    {
        currState.Update();
    }
}
