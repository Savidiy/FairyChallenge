namespace Fairy
{
    public sealed class WinFightState : IFightState, IState
    {
        private readonly FightLoop _fightLoop;

        public WinFightState(FightLoop fightLoop)
        {
            _fightLoop = fightLoop;
        }

        public void Enter()
        {
            _fightLoop.Complete(FightResult.Win);
        }
    }
}