
namespace AFSInterview
{
    public interface IMoneyListener
    {
        void AttachObserver(IObserver observer);

        void DetachObserver(IObserver observer);

        void NotifyObservers();
    }
}
