namespace Fairy
{
    public sealed class EncounterStepFactory : IConcreteStepFactory
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        public StepType Type { get; } = StepType.Encounter;

        public EncounterStepFactory(ApplicationStateMachine applicationStateMachine)
        {
            _applicationStateMachine = applicationStateMachine;
        }

        public IStep Create(StepStaticData stepStaticData)
        {
            return new EncounterStep(stepStaticData.EncounterId, stepStaticData.WinNodeId, stepStaticData.LoseNodeId, _applicationStateMachine);
        }
    }
}