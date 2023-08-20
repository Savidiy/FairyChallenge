using System.Threading;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public class GotoStep : IStep
    {
        private readonly string _nodeId;
        private readonly StoryWindow _storyWindow;

        public GotoStep(string nodeId, StoryWindow storyWindow)
        {
            _nodeId = nodeId;
            _storyWindow = storyWindow;
        }

        public UniTask Execute(CancellationToken token)
        {
            _storyWindow.ActionButtons.SendClick(_nodeId);
            return UniTask.CompletedTask;
        }
    }
}