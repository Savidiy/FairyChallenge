using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Fairy
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        private ApplicationStateMachine _mainStateMachine;

        [Inject]
        public void Construct(ApplicationStateMachine applicationStateMachine, List<IApplicationState> applicationStates)
        {
            _mainStateMachine = applicationStateMachine;
            _mainStateMachine.AddStates(applicationStates);
        }

        public void Start()
        {
            _mainStateMachine.EnterToState<IntroApplicationState>();
        }
    }
}