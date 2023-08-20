namespace Fairy
{
    public class StoryApplicationState : IApplicationState, IState, IStateWithExit
    {
        private readonly StoryWindow _storyWindow;

        public StoryApplicationState(StoryWindow storyWindow)
        {
            _storyWindow = storyWindow;
        }

        public void Enter()
        {
            _storyWindow.Show();
        }

        public void Exit()
        {
            _storyWindow.Hide();
        }
    }
}