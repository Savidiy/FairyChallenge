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

    public class FightApplicationState : IApplicationState, IStateWithPayload<FightPayload>, IStateWithExit
    {
        private readonly FightWindow _fightWindow;
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly StoryTeller _storyTeller;
        private FightPayload _payload;

        public FightApplicationState(FightWindow fightWindow, ApplicationStateMachine applicationStateMachine,
            StoryTeller storyTeller)
        {
            _fightWindow = fightWindow;
            _applicationStateMachine = applicationStateMachine;
            _storyTeller = storyTeller;
        }

        public void Enter(FightPayload payload)
        {
            _payload = payload;
            ShowIntroWindow().Forget();
        }

        private async UniTaskVoid ShowIntroWindow()
        {
            bool isWin = await _fightWindow.ShowAsync();
            await _fightWindow.HideAsync();

            _storyTeller.SetCurrentNodeId(isWin ? _payload.WinNodeId : _payload.LoseNodeId);
            _applicationStateMachine.EnterToState<StoryApplicationState>();
        }

        public void Exit()
        {
            _fightWindow.Hide();
        }
    }
}