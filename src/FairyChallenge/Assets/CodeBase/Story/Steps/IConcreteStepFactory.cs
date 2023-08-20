namespace Fairy
{
    public interface IConcreteStepFactory
    {
        StepType Type { get; }
        IStep Create(StepStaticData stepStaticData);
    }
}