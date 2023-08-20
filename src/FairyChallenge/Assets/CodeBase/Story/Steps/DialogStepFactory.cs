namespace Fairy
{
    public sealed class DialogStepFactory : IConcreteStepFactory
    {
        private readonly StoryWindow _storyWindow;
        private readonly PersonLibrary _personLibrary;
        public StepType Type { get; } = StepType.Dialog;

        public DialogStepFactory(StoryWindow storyWindow, PersonLibrary personLibrary)
        {
            _storyWindow = storyWindow;
            _personLibrary = personLibrary;
        }

        public IStep Create(StepStaticData stepStaticData)
        {
            PersonStaticData personStaticData = _personLibrary.GetPersonData(stepStaticData.PersonId);
            return new DialogStep(personStaticData, stepStaticData.Text, _storyWindow);
        }
    }
}