namespace Fairy
{
    public class NewGameApplicationState : IApplicationState, IState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly StoryTeller _storyTeller;
        private readonly StorySettings _storySettings;

        public NewGameApplicationState(ApplicationStateMachine applicationStateMachine, StoryTeller storyTeller,
            StorySettings storySettings)
        {
            _applicationStateMachine = applicationStateMachine;
            _storyTeller = storyTeller;
            _storySettings = storySettings;
        }

        public void Enter()
        {
            // drop progress
            _storyTeller.SetCurrentNodeId(_storySettings.FirstNodeId);
            _applicationStateMachine.EnterToState<StoryApplicationState>();
        }
    }
}