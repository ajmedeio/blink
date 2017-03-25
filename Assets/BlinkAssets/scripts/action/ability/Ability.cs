using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : Action {

	// Anti magic bubble that reduces locked on abilities to simple projectiles

	// Abilities should use basic shapes that are instantiated and thrown at a target location
	// or player, and then destroyed on impact. The projectiles speed should be at least twice
	// as fast as player movement.

	// Taunt ability that puts a debuff on the target which increases damage done to the target
	// by the taunter until the tauntee

	public static readonly Ability blink = new Blink();

	public Ability(string name) : base(name) {
	}

	public Ability (string name, string description, float range, float initialDamage, string initialAnimation, 
		float radius, float dotTickInterval, float dotDuration, float dotDamage, string dotAnimation, 
		float terminalDamage, string terminalAnimation) : base(name) {

	}

	public abstract bool IsLegal (HeroManager hero);

	public abstract void DoAbility (HeroManager hero);

}

