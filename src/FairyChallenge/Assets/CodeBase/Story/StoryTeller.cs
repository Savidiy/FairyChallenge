using System.Threading;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public sealed class StoryTeller
    {
        private readonly StepFactory _stepFactory;
        private readonly NodesLibrary _nodesLibrary;
        private readonly StoryWindow _storyWindow;
        
        private string _currentNodeId;
        private CancellationTokenSource _cancellationTokenSource;

        public StoryTeller(StepFactory stepFactory, NodesLibrary nodesLibrary, StoryWindow storyWindow)
        {
            _stepFactory = stepFactory;
            _nodesLibrary = nodesLibrary;
            _storyWindow = storyWindow;
            _storyWindow.ActionButtons.NodeClicked += OnNodeClicked;
        }

        private void OnNodeClicked(string nodeId)
        {
            _storyWindow.ActionButtons.HideButtons();
            PlayNode(nodeId);    
        }

        public void SetCurrentNodeId(string currentNodeId)
        {
            _currentNodeId = currentNodeId;
        }

        private void PlayNode(string nodeId)
        {
            SetCurrentNodeId(nodeId);
            PlayCurrentNode().Forget();
        }

        public async UniTask PlayCurrentNode()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            NodeStaticData node = _nodesLibrary.GetNodeStaticData(_currentNodeId);
            foreach (StepStaticData stepStaticData in node.Steps)
            {
                var step = _stepFactory.Create(stepStaticData);
                await step.Execute(_cancellationTokenSource.Token);
            }
        }
    }
}