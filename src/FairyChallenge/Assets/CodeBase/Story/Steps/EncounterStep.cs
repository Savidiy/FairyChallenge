using System.Threading;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public class EncounterStep : IStep
    {
        private readonly string _encounterId;
        private readonly string _winNodeId;
        private readonly string _loseNodeId;
        private readonly ApplicationStateMachine _applicationStateMachine;

        public EncounterStep(string encounterId, string winNodeId, string loseNodeId,
            ApplicationStateMachine applicationStateMachine)
        {
            _encounterId = encounterId;
            _winNodeId = winNodeId;
            _loseNodeId = loseNodeId;
            _applicationStateMachine = applicationStateMachine;
        }

        public UniTask Execute(CancellationToken token)
        {
            var payload = new FightPayload(_encounterId, _winNodeId, _loseNodeId);
            _applicationStateMachine.EnterToState<FightApplicationState, FightPayload>(payload);
            return UniTask.CompletedTask;
        }
    }
}