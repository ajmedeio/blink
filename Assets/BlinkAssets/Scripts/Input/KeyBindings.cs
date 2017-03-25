using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindings {

	public Dictionary<KeyCode, Action> keyDownBindings;
	public Dictionary<KeyCode, Action> keyUpBindings;
	public Dictionary<KeyCode, Action> keyBindings;

	public KeyBindings() {
		keyDownBindings = new Dictionary<KeyCode, Action> ();
		keyUpBindings = new Dictionary<KeyCode, Action> ();
		keyBindings = new Dictionary<KeyCode, Action> ();
	}

	public void SetDefaultKeyboardMouseBindings() {

		// hero movement bindings
		keyBindings.Add (KeyCode.X, MovementAction.MinusY);
		keyBindings.Add (KeyCode.S, MovementAction.MinusZ);
		keyBindings.Add (KeyCode.Q, MovementAction.MinusX);
		keyBindings.Add (KeyCode.E, MovementAction.PlusX);
		keyBindings.Add (KeyCode.W, MovementAction.PlusZ);
		keyBindings.Add (KeyCode.A, MovementAction.MinusYRotate);
		keyBindings.Add (KeyCode.D, MovementAction.PlusYRotate);
		keyBindings.Add (KeyCode.Mouse0, MovementAction.ChangeCameraAngle);
		keyBindings.Add (KeyCode.Mouse1, MovementAction.ChangeHeroAngle);

		keyDownBindings.Add (KeyCode.Space, MovementAction.PlusY);
		keyDownBindings.Add (KeyCode.T, MovementAction.ToggleRunWalk);

		// ability bindings
		keyUpBindings.Add (KeyCode.Alpha1, Ability.blink);

		// hero HUD bindings
		keyDownBindings.Add (KeyCode.Return, HudAction.ToggleTyping);

	}

	public void FromJson(string toParse) {

	}
}
