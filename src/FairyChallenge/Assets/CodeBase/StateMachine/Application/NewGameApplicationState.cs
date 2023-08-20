namespace Fairy
{
    public class NewGameApplicationState : IApplicationState, IState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly StoryTeller _storyTeller;
        private readonly StorySettings _storySettings;
        private readonly PlayerHandler _playerHandler;
        private readonly HeroFactory _heroFactory;

        public NewGameApplicationState(ApplicationStateMachine applicationStateMachine, StoryTeller storyTeller,
            StorySettings storySettings, PlayerHandler playerHandler, HeroFactory heroFactory)
        {
            _applicationStateMachine = applicationStateMachine;
            _storyTeller = storyTeller;
            _storySettings = storySettings;
            _playerHandler = playerHandler;
            _heroFactory = heroFactory;
        }

        public void Enter()
        {
            // drop progress
            _storyTeller.SetCurrentNodeId(_storySettings.FirstNodeId);

            Hero hero = _heroFactory.Create(_storySettings.StartHeroId);
            _playerHandler.SetHero(hero);
            _applicationStateMachine.EnterToState<StoryApplicationState>();
        }
    }
}