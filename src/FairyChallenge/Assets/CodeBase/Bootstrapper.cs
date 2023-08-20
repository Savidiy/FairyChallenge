using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Fairy
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        private ApplicationStateMachine _mainStateMachine;

        [Inject]
        public void Construct(ApplicationStateMachine applicationStateMachine, List<IApplicationState> applicationStates,
            FightStateMachine fightStateMachine, List<IFightState> fightStates)
        {
            _mainStateMachine = applicationStateMachine;
            _mainStateMachine.AddStates(applicationStates);

            fightStateMachine.AddStates(fightStates);
        }

        public void Start()
        {
            _mainStateMachine.EnterToState<IntroApplicationState>();
        }
    }
}