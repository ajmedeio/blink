using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, IObservable {

	private HashSet<IObserver> observers;
	private KeyBindings bindings;
	private HashSet<Action> actions;

	void Start () {
		observers = new HashSet<IObserver> ();
		observers.Add (GetComponent<Hero> ());

		bindings = new KeyBindings ();
		bindings.SetDefaultKeyboardMouseBindings ();

		actions = new HashSet<Action> ();
	}

	private void Update() {
		if (bindings == null) return; // for some reason this is null sometimes in the middle of gameplay

		foreach (KeyValuePair<KeyCode, Action> kv in bindings.keyUpBindings) {
			if (Input.GetKeyUp (kv.Key)) {
				actions.Add (kv.Value);
			}
		}
		foreach (KeyValuePair<KeyCode, Action> kv in bindings.keyDownBindings) {
			if (Input.GetKeyDown (kv.Key)) {
				actions.Add (kv.Value);
			}
		}
	}

	// Fixed update is called in sync with physics
	void FixedUpdate () {
		// This is a lame way to do things, but it's the only way you can in Unity.
		// Iterate over every key in bindings and compare any input with bound keys.
		// notify any controller observers (usually a hero) of actions mapped to
		// the pressed keys
		if (bindings == null) return; // for some reason this is null sometimes in the middle of gameplay

		foreach (KeyValuePair<KeyCode, Action> kv in bindings.keyBindings) {
			if (Input.GetKey (kv.Key)) {
				//print (string.Format ("Controller.cs: {0} was pressed and processed in Update", kv.Value));
				actions.Add (kv.Value);
			}
		}

		foreach (IObserver o in observers) o.Notify (this, actions);

		actions.Clear ();
	}

	void IObservable.SetObserver(IObserver observer) {
		observers.Add(observer);
	}

	void IObservable.RemoveObserver(IObserver observer) {
		observers.Remove(observer);
	}

}
