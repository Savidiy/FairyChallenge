using System.Threading;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public sealed class BackgroundStep : IStep
    {
        private readonly string _backgroundId;
        private readonly StoryWindow _storyWindow;
        private readonly BackgroundLibrary _backgroundLibrary;

        public BackgroundStep(string backgroundId, StoryWindow storyWindow, BackgroundLibrary backgroundLibrary)
        {
            _backgroundId = backgroundId;
            _storyWindow = storyWindow;
            _backgroundLibrary = backgroundLibrary;
        }

        public UniTask Execute(CancellationToken token)
        {
            _storyWindow.SetBackground(_backgroundLibrary.GetBackground(_backgroundId));
            return UniTask.CompletedTask;
        }
    }
}