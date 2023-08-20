using UnityEngine;

namespace Fairy
{
    public sealed class SelectActionFightState : IFightState, IState, IStateWithExit
    {
        private readonly FightStateMachine _fightStateMachine;
        private readonly FightWindow _fightWindow;
        private readonly PlayerHandler _playerHandler;
        private readonly EnemyHandler _enemyHandler;

        public SelectActionFightState(FightStateMachine fightStateMachine, FightWindow fightWindow,
            PlayerHandler playerHandler, EnemyHandler enemyHandler)
        {
            _fightStateMachine = fightStateMachine;
            _fightWindow = fightWindow;
            _playerHandler = playerHandler;
            _enemyHandler = enemyHandler;
        }

        public void Enter()
        {
            _fightWindow.ShowActions(_playerHandler.Hero);
            _fightWindow.ActionSelected += OnActionSelected;
        }

        private void OnActionSelected(int heroActionIndex)
        {
            int enemyActionIndex = SelectEnemyActionIndex();
            var payload = new ActionPayload(heroActionIndex, enemyActionIndex);
            _fightStateMachine.EnterToState<ActionFightState, ActionPayload>(payload);
        }

        public void Exit()
        {
            _fightWindow.ActionSelected -= OnActionSelected;
            _fightWindow.HideActions();
        }

        private int SelectEnemyActionIndex()
        {
            Hero enemy = _enemyHandler.Enemy;
            int enemyActionCount = enemy.HeroActions.Count + enemy.Inventory.Consumables.Count;
            int enemyAction = Random.Range(0, enemyActionCount);
            return enemyAction;
        }
    }
}