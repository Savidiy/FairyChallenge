using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public sealed class ActionPayload
    {
        public int HeroActionIndex { get; }
        public int EnemyActionIndex { get; }

        public ActionPayload(int heroActionIndex, int enemyActionIndex)
        {
            HeroActionIndex = heroActionIndex;
            EnemyActionIndex = enemyActionIndex;
        }
    }

    public sealed class ActionFightState : IFightState, IStateWithPayload<ActionPayload>
    {
        private readonly FightCalculator _fightCalculator;
        private readonly FightWindow _fightWindow;
        private readonly PlayerHandler _playerHandler;
        private readonly EnemyHandler _enemyHandler;
        private readonly FightStateMachine _fightStateMachine;

        public ActionFightState(FightCalculator fightCalculator, FightWindow fightWindow, PlayerHandler playerHandler,
            EnemyHandler enemyHandler, FightStateMachine fightStateMachine)
        {
            _fightCalculator = fightCalculator;
            _fightWindow = fightWindow;
            _playerHandler = playerHandler;
            _enemyHandler = enemyHandler;
            _fightStateMachine = fightStateMachine;
        }

        public void Enter(ActionPayload payload)
        {
            MakeFight(payload).Forget();
        }

        private async UniTask MakeFight(ActionPayload payload)
        {
            Hero hero = _playerHandler.Hero;
            Hero enemy = _enemyHandler.Enemy;
            var heroes = new List<Hero> {hero, enemy};
            int heroActionIndex = payload.HeroActionIndex;
            int enemyActionIndex = payload.EnemyActionIndex;

            if (_fightCalculator.TryCalcResult(hero, heroActionIndex, enemy, out ActionResult actionResult))
            {
                ApplyResult(actionResult, heroes);
                await ShowActionResult(hero, enemy, actionResult);
            }

            if (IsEndFight(hero, enemy))
            {
                EndFight(hero, enemy);
            }
            else if (_fightCalculator.TryCalcResult(enemy, enemyActionIndex, hero, out ActionResult enemyActionResult))
            {
                ApplyResult(enemyActionResult, heroes);
                await ShowActionResult(hero, enemy, actionResult);
            }

            if (IsEndFight(hero, enemy))
                EndFight(hero, enemy);
            else
                _fightStateMachine.EnterToState<SelectActionFightState>();
        }

        private async UniTask ShowActionResult(Hero hero, Hero enemy, ActionResult actionResult)
        {
            _fightWindow.ShowActionResult(hero, enemy, actionResult);
            await UniTask.Delay(500);
        }

        private bool IsEndFight(Hero hero, Hero enemy)
        {
            return !enemy.IsAlive || !hero.IsAlive;
        }

        private void EndFight(Hero hero, Hero enemy)
        {
            if (!hero.IsAlive)
            {
                _fightStateMachine.EnterToState<LoseFightState>();
            }
            else if (!enemy.IsAlive)
            {
                _fightStateMachine.EnterToState<WinFightState>();
            }
        }
        
        private static void ApplyResult(ActionResult actionResult, List<Hero> heroes)
        {
            foreach (Hero hero in heroes)
            foreach (StatChangeData heroChange in actionResult.GetChanges(hero))
                hero.Stats.ApplyChange(heroChange);
        }
    }
}