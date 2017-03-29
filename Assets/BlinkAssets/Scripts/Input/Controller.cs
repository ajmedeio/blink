using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controller : NetworkBehaviour, IObservable {

	private HashSet<IObserver> observers;
	private KeyBindings bindings;
	private HashSet<MovementAction> movementActions;
	private HashSet<HudAction> hudActions;
	private HashSet<Ability> abilities;

	void Start () {
		observers = new HashSet<IObserver> ();

		// I don't know how to ensure the Controller is instantiated before the HeroMovement and HeroHud in which case
		// I would call controller.AddObserver(heroMovement) & controller.AddObserver(heroHud) inside their respective
		// classes, but this works for now.
		observers.Add (GetComponent<HeroMovement> ());
		observers.Add (GetComponent<HeroCombat> ());
		observers.Add (GetComponent<HeroHud> ());

		bindings = new KeyBindings ();
		bindings.SetDefaultKeyboardMouseBindings ();

		movementActions = new HashSet<MovementAction> ();
		hudActions = new HashSet<HudAction> ();
		abilities = new HashSet<Ability> ();
	}

	private void Update() {
		if (!isLocalPlayer) return;

		if (bindings == null) return; // for some reason this is null sometimes in the middle of gameplay

		foreach (KeyValuePair<KeyCode, Action> kv in bindings.keyUpBindings) {
			if (Input.GetKeyUp (kv.Key)) {
				AddActionToCorrectMap(kv);
			}
		}
		foreach (KeyValuePair<KeyCode, Action> kv in bindings.keyDownBindings) {
			if (Input.GetKeyDown (kv.Key)) {
				AddActionToCorrectMap(kv);
			}
		}
	}

	// Fixed update is called in sync with physics
	void FixedUpdate () {
		if (!isLocalPlayer) return;
		if (bindings == null) return; // for some reason this is null sometimes in the middle of gameplay

		foreach (KeyValuePair<KeyCode, Action> kv in bindings.keyBindings) {
			if (Input.GetKey (kv.Key)) {
				//print (string.Format ("Controller.cs: {0} was pressed and processed in Update", kv.Value));
				AddActionToCorrectMap(kv);
			}
		}

		foreach (IObserver o in observers) o.Notify (this, movementActions);
		foreach (IObserver o in observers) o.Notify (this, hudActions);
		foreach (IObserver o in observers) o.Notify (this, abilities);

		movementActions.Clear ();
		hudActions.Clear ();
		abilities.Clear ();
	}

	void AddActionToCorrectMap(KeyValuePair<KeyCode, Action> kv) {
		if (kv.Value is MovementAction)
			movementActions.Add (kv.Value as MovementAction);
		else if (kv.Value is HudAction)
			hudActions.Add (kv.Value as HudAction);
		else if (kv.Value is Ability)
			abilities.Add (kv.Value as Ability);
	}

	void IObservable.AddObserver(IObserver observer) {
		observers.Add(observer);
	}

	void IObservable.RemoveObserver(IObserver observer) {
		observers.Remove(observer);
	}

}
