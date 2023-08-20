using Cysharp.Threading.Tasks;

namespace Fairy
{
    public sealed class FightPayload
    {
        public string EncounterId { get; }
        public string WinNodeId { get; }
        public string LoseNodeId { get; }

        public FightPayload(string encounterId, string winNodeId, string loseNodeId)
        {
            EncounterId = encounterId;
            WinNodeId = winNodeId;
            LoseNodeId = loseNodeId;
        }
    }

    public sealed class FightApplicationState : IApplicationState, IStateWithPayload<FightPayload>, IStateWithExit
    {
        private readonly FightWindow _fightWindow;
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly StoryTeller _storyTeller;
        private readonly FightLoop _fightLoop;
        private FightPayload _payload;

        public FightApplicationState(FightWindow fightWindow, ApplicationStateMachine applicationStateMachine,
            StoryTeller storyTeller, FightLoop fightLoop)
        {
            _fightWindow = fightWindow;
            _applicationStateMachine = applicationStateMachine;
            _storyTeller = storyTeller;
            _fightLoop = fightLoop;
        }

        public void Enter(FightPayload payload)
        {
            _payload = payload;
            FightAsync().Forget();
        }

        private async UniTaskVoid FightAsync()
        {
            FightResult fightResult = await _fightLoop.StartAsync(_payload.EncounterId);

            if (fightResult == FightResult.RestartGame)
            {
                _applicationStateMachine.EnterToState<NewGameApplicationState>();
            }
            else
            {
                _storyTeller.SetCurrentNodeId(fightResult == FightResult.Win ? _payload.WinNodeId : _payload.LoseNodeId);
                _applicationStateMachine.EnterToState<StoryApplicationState>();
            }
        }

        public void Exit()
        {
            _fightWindow.Hide();
        }
    }
}