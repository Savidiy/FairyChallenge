using System.Threading;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public class StoryTeller
    {
        private readonly StepFactory _stepFactory;
        private readonly NodesLibrary _nodesLibrary;
        private string _currentNodeId;
        private CancellationTokenSource _cancellationTokenSource;

        public StoryTeller(StepFactory stepFactory, NodesLibrary nodesLibrary)
        {
            _stepFactory = stepFactory;
            _nodesLibrary = nodesLibrary;
        }

        public void SetCurrentNodeId(string currentNodeId)
        {
            _currentNodeId = currentNodeId;
        }

        public void PlayNode(string nodeId)
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