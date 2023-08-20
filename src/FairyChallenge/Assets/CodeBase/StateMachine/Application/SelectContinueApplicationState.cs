namespace Fairy
{
    public class SelectContinueApplicationState : IApplicationState, IState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;

        public SelectContinueApplicationState(ApplicationStateMachine applicationStateMachine)
        {
            _applicationStateMachine = applicationStateMachine;
        }
        
        public void Enter()
        {
            // show window if has progress 
            // continue
            // new game
            _applicationStateMachine.EnterToState<NewGameApplicationState>();
        }
    }
}