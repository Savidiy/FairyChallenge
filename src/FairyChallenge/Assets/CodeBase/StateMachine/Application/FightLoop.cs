using Cysharp.Threading.Tasks;

namespace Fairy
{
    public sealed class FightLoop
    {
        private readonly FightStateMachine _fightStateMachine;
        private UniTaskCompletionSource<FightResult> _completionSource;

        public FightLoop(FightStateMachine fightStateMachine)
        {
            _fightStateMachine = fightStateMachine;
        }

        public UniTask<FightResult> StartAsync(string encounterId)
        {
            _completionSource = new UniTaskCompletionSource<FightResult>();
            _fightStateMachine.EnterToState<LoadingFightState, string>(encounterId);
            return _completionSource.Task;
        }

        public void Complete(FightResult fightResult)
        {
            _fightStateMachine.EnterToState<OffFightState>();
            _completionSource.TrySetResult(fightResult);
        }
    }
}