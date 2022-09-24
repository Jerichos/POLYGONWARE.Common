namespace Common
{
    public interface IControllable
    {
        void TakeControl(IControllable owner);
    }
}