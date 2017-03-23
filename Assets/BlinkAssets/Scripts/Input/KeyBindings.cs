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
		keyBindings.Add (KeyCode.X, HeroAction.MinusY);
		keyBindings.Add (KeyCode.S, HeroAction.MinusZ);
		keyBindings.Add (KeyCode.Q, HeroAction.MinusX);
		keyBindings.Add (KeyCode.E, HeroAction.PlusX);
		keyBindings.Add (KeyCode.W, HeroAction.PlusZ);
		keyBindings.Add (KeyCode.A, HeroAction.MinusYRotate);
		keyBindings.Add (KeyCode.D, HeroAction.PlusYRotate);
		keyBindings.Add (KeyCode.Mouse0, HeroAction.ChangeCameraAngle);
		keyBindings.Add (KeyCode.Mouse1, HeroAction.ChangeHeroAngle);

		keyDownBindings.Add (KeyCode.Space, HeroAction.PlusY);
		keyDownBindings.Add (KeyCode.T, HeroAction.ToggleRunWalk);

		// ability bindings
		keyUpBindings.Add (KeyCode.Alpha1, Abilities.blink);

		// hero HUD bindings
		keyDownBindings.Add (KeyCode.Return, GuiAction.ToggleTyping);

	}

	public void FromJson(string toParse) {

	}
}
