using Cysharp.Threading.Tasks;

namespace Fairy
{
    public sealed class OffFightState : IFightState, IState
    {
        private readonly FightWindow _fightWindow;

        public OffFightState(FightWindow fightWindow)
        {
            _fightWindow = fightWindow;
        }

        public void Enter()
        {
            _fightWindow.HideAsync().Forget();
        }
    }
}