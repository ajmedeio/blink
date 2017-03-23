public interface IObserver {
	void Notify(IObservable observable, object msg);
}
