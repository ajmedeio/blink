public interface IObservable {
	void SetObserver(IObserver observer);
	void RemoveObserver(IObserver observer);
}
