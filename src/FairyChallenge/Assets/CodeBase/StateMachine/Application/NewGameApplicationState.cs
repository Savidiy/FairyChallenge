namespace Fairy
{
    public class NewGameApplicationState : IApplicationState, IState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;

        public NewGameApplicationState(ApplicationStateMachine applicationStateMachine)
        {
            _applicationStateMachine = applicationStateMachine;
        }

        public void Enter()
        {
            // drop progress
            _applicationStateMachine.EnterToState<StoryApplicationState>();
        }
    }
}