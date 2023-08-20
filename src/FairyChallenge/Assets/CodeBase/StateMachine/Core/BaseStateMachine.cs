using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Fairy
{
    public abstract class BaseStateMachine<T> : IDisposable
        where T : class
    {
        [CanBeNull] public T CurrentState { get; private set; }

        private readonly Dictionary<Type, T> _states = new();

        public void AddStates(IReadOnlyList<T> states)
        {
            foreach (T state in states)
                _states.Add(state.GetType(), state);
        }
        
        public void AddState<TState>(TState state)
            where TState : T, IState
        {
            _states.Add(state.GetType(), state);
        }

        public void AddState<TState, TPayload>(TState state)
            where TState : T, IStateWithPayload<TPayload>
        {
            _states.Add(state.GetType(), state);
        }

        public void EnterToState<TType>()
            where TType : T, IState
        {
            ExitFromCurrentState();

            Type stateType = typeof(TType);
            T state = GetState(stateType);
            CurrentState = state;

            if (state is not IState concreteState)
                throw new Exception($"There is type '{stateType}' without '{nameof(IState)}' interface");

            concreteState.Enter();
        }

        public void EnterToState<TType, TPayload>(TPayload payload)
            where TType : T, IStateWithPayload<TPayload>
        {
            ExitFromCurrentState();

            Type stateType = typeof(TType);
            T state = GetState(stateType);
            CurrentState = state;

            if (state is not IStateWithPayload<TPayload> stateWithoutPayload)
                throw new Exception($"There is type '{stateType}' without '{nameof(IStateWithPayload<TPayload>)}' interface");

            stateWithoutPayload.Enter(payload);
        }

        private T GetState(Type stateType)
        {
            if (!_states.TryGetValue(stateType, out T state))
                throw new Exception($"There is not state with type '{stateType}'");

            return state;
        }

        private void ExitFromCurrentState()
        {
            if (CurrentState is IStateWithExit stateWithExit)
                stateWithExit.Exit();
        }

        public void Dispose()
        {
            foreach ((Type _, T state) in _states)
                if (state is IDisposable disposable)
                    disposable.Dispose();

            _states.Clear();

            CurrentState = null;
        }
    }
}