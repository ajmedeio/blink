public interface IObservable {
	void AddObserver(IObserver observer);
	void RemoveObserver(IObserver observer);
}
