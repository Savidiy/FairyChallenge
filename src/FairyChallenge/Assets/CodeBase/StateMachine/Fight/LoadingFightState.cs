namespace Fairy
{
    public sealed class LoadingFightState : IFightState, IStateWithPayload<string>
    {
        private readonly EncounterLibrary _encounterLibrary;
        private readonly PlayerHandler _playerHandler;
        private readonly EnemyHandler _enemyHandler;
        private readonly FightStateMachine _fightStateMachine;
        private readonly HeroFactory _heroFactory;

        public LoadingFightState(EncounterLibrary encounterLibrary, PlayerHandler playerHandler, EnemyHandler enemyHandler,
            FightStateMachine fightStateMachine, HeroFactory heroFactory)
        {
            _encounterLibrary = encounterLibrary;
            _playerHandler = playerHandler;
            _enemyHandler = enemyHandler;
            _fightStateMachine = fightStateMachine;
            _heroFactory = heroFactory;
        }

        public void Enter(string encounterId)
        {
            _playerHandler.SaveHeroBeforeFight();
            CreateEnemy(encounterId);
            _fightStateMachine.EnterToState<IntroFightState>();
        }

        private void CreateEnemy(string encounterId)
        {
            EncounterStaticData encounterStaticData = _encounterLibrary.GetStaticData(encounterId);
            string enemyId = encounterStaticData.EnemyId;
            Hero enemy = _heroFactory.Create(enemyId);
            _enemyHandler.SetEnemy(enemy);
        }
    }
}