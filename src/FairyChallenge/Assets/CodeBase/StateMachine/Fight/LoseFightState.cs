namespace Fairy
{
    public sealed class LoseFightState : IFightState, IState, IStateWithExit
    {
        private readonly FightLoop _fightLoop;
        private readonly PlayerHandler _playerHandler;
        private readonly FightWindow _fightWindow;

        public LoseFightState(FightLoop fightLoop, PlayerHandler playerHandler, FightWindow fightWindow)
        {
            _fightWindow = fightWindow;
            _fightLoop = fightLoop;
            _playerHandler = playerHandler;
        }

        public void Enter()
        {
            _fightWindow.ShowLose();
            _fightWindow.RestartClicked += OnRestartClicked;
            _fightWindow.RepeatClicked += OnRepeatClicked;
        }

        private void OnRepeatClicked()
        {
            _playerHandler.LoadHeroForRepeatFight();
            _fightLoop.Complete(FightResult.Lose);
        }

        private void OnRestartClicked()
        {
            _fightLoop.Complete(FightResult.RestartGame);
        }

        public void Exit()
        {
            _fightWindow.HideLose();
            _fightWindow.RestartClicked -= OnRestartClicked;
            _fightWindow.RepeatClicked -= OnRepeatClicked;
        }
    }
}