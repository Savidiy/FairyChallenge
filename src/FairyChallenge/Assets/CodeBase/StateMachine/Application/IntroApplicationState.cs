using Cysharp.Threading.Tasks;

namespace Fairy
{
    public class IntroApplicationState : IApplicationState, IState, IStateWithExit
    {
        private readonly IntroWindow _introWindow;
        private readonly ApplicationStateMachine _applicationStateMachine;

        public IntroApplicationState(IntroWindow introWindow, ApplicationStateMachine applicationStateMachine)
        {
            _introWindow = introWindow;
            _applicationStateMachine = applicationStateMachine;
        }

        public void Enter()
        {
            ShowIntroWindow().Forget();
        }

        private async UniTaskVoid ShowIntroWindow()
        {
            await _introWindow.ShowAsync();
            await _introWindow.HideAsync();
            _applicationStateMachine.EnterToState<SelectContinueApplicationState>();
        }

        public void Exit()
        {
            _introWindow.Hide();
        }
    }
}