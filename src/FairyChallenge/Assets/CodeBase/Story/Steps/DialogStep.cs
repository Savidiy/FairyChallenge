using System.Threading;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public class DialogStep : IStep
    {
        private readonly PersonStaticData _personStaticData;
        private readonly string _text;
        private readonly StoryWindow _storyWindow;

        public DialogStep(PersonStaticData personStaticData, string text, StoryWindow storyWindow)
        {
            _storyWindow = storyWindow;
            _text = text;
            _personStaticData = personStaticData;
        }

        public UniTask Execute(CancellationToken token)
        {
            return _storyWindow.ShowDialogAsync(_personStaticData, _text, token);
        }
    }
}