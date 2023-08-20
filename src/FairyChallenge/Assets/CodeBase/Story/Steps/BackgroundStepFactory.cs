namespace Fairy
{
    public sealed class BackgroundStepFactory : IConcreteStepFactory
    {
        private readonly StoryWindow _storyWindow;
        private readonly BackgroundLibrary _backgroundLibrary;
        public StepType Type { get; } = StepType.Background;

        public BackgroundStepFactory(StoryWindow storyWindow, BackgroundLibrary backgroundLibrary)
        {
            _storyWindow = storyWindow;
            _backgroundLibrary = backgroundLibrary;
        }

        public IStep Create(StepStaticData stepStaticData)
        {
            return new BackgroundStep(stepStaticData.BackgroundId, _storyWindow, _backgroundLibrary);
        }
    }
}