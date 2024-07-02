public class State<T> where T : class
{
    protected StateMachine<T> m_stateMachine;

    public State(StateMachine<T> stateMachine)
    {
        m_stateMachine = stateMachine;
    }

    public virtual void Enter_State()
    {
    }

    public virtual void Update_State()
    {
    }

    public virtual void Exit_State()
    {
    }

    public virtual void OnDrawGizmos()
    {
    }
}