using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities {

	// cannot instantiate the factory
	private Abilities() {}

	public static readonly Ability blink = new Blink();

	// Anti magic bubble that reduces locked on abilities to simple projectiles

	// Abilities should use basic shapes that are instantiated and thrown at a target location
	// or player, and then destroyed on impact. The projectiles speed should be at least twice
	// as fast as player movement.

	// Taunt ability that puts a debuff on the target which increases damage done to the target
	// by the taunter until the tauntee 

}
