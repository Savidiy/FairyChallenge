using System.Threading;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public class ButtonStep : IStep
    {
        private readonly string _buttonText;
        private readonly string _nextNodeId;
        private readonly StoryWindow _storyWindow;

        public ButtonStep(string buttonText, string nextNodeId, StoryWindow storyWindow)
        {
            _buttonText = buttonText;
            _nextNodeId = nextNodeId;
            _storyWindow = storyWindow;
        }

        public UniTask Execute(CancellationToken token)
        {
            _storyWindow.ActionButtons.AddButton(_buttonText, _nextNodeId);
            return UniTask.CompletedTask;
        }
    }
}