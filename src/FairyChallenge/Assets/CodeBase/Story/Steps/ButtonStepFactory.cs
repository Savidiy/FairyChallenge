namespace Fairy
{
    public sealed class ButtonStepFactory : IConcreteStepFactory
    {
        private readonly StoryWindow _storyWindow;
        public StepType Type { get; } = StepType.Button;

        public ButtonStepFactory(StoryWindow storyWindow)
        {
            _storyWindow = storyWindow;
        }

        public IStep Create(StepStaticData stepStaticData)
        {
            return new ButtonStep(stepStaticData.ButtonText, stepStaticData.NodeId, _storyWindow);
        }
    }
}