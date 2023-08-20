namespace Fairy
{
    public sealed class GotoStepFactory : IConcreteStepFactory
    {
        private readonly StoryWindow _storyWindow;
        public StepType Type { get; } = StepType.Goto;

        public GotoStepFactory(StoryWindow storyWindow)
        {
            _storyWindow = storyWindow;
        }

        public IStep Create(StepStaticData stepStaticData)
        {
            return new GotoStep(stepStaticData.NodeId, _storyWindow);
        }
    }
}