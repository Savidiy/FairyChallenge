using Cysharp.Threading.Tasks;

namespace Fairy
{
    public sealed class StoryApplicationState : IApplicationState, IState, IStateWithExit
    {
        private readonly StoryWindow _storyWindow;
        private readonly StoryTeller _storyTeller;

        public StoryApplicationState(StoryWindow storyWindow, StoryTeller storyTeller)
        {
            _storyWindow = storyWindow;
            _storyTeller = storyTeller;
        }

        public void Enter()
        {
            _storyWindow.Show();
            _storyTeller.PlayCurrentNode().Forget();
        }

        public void Exit()
        {
            _storyWindow.Hide();
        }
    }
}