using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities {

	// cannot instantiate the factory
	private Abilities() {}

	public static readonly Ability blink = new Blink();

}
