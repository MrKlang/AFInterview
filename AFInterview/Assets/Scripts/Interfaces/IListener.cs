namespace AFSInterview
{
    public interface IListener
    {
        void AttachObserver(IObserver observer);

        void DetachObserver(IObserver observer);

        void NotifyObservers();
    }
}
