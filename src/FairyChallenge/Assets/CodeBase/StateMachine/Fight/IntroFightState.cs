using Cysharp.Threading.Tasks;

namespace Fairy
{
    public sealed class IntroFightState : IFightState, IState
    {
        private readonly FightWindow _fightWindow;
        private readonly PlayerHandler _playerHandler;
        private readonly EnemyHandler _enemyHandler;
        private readonly FightStateMachine _fightStateMachine;

        public IntroFightState(FightWindow fightWindow, PlayerHandler playerHandler, EnemyHandler enemyHandler,
            FightStateMachine fightStateMachine)
        {
            _fightWindow = fightWindow;
            _playerHandler = playerHandler;
            _enemyHandler = enemyHandler;
            _fightStateMachine = fightStateMachine;
        }

        public void Enter()
        {
            _fightWindow.Initialize(_playerHandler.Hero, _enemyHandler.Enemy);
            _fightWindow.ShowAsync().Forget();
            _fightStateMachine.EnterToState<SelectActionFightState>();
        }
    }
}