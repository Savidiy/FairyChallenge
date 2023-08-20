namespace Fairy
{
    public interface IStateWithPayload<in T>
    {
        void Enter(T payload);
    }
}