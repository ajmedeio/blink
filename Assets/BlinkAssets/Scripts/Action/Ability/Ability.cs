using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Ability : Action {

	// Anti magic bubble that reduces locked on abilities to simple projectiles

	// Abilities should use basic shapes that are instantiated and thrown at a target location
	// or player, and then destroyed on impact. The projectiles speed should be at least twice
	// as fast as player movement.

	// Taunt ability that puts a debuff on the target which increases damage done to the target
	// by the taunter until the tauntee

	// Teleportation that every player has after visiting some person or trainer or zone.

	// Abilities shouldn't be given through some UI, you should have to have the ability trained
	// by masters of that class of abilities
	// then you learn the ability and it's added to your spellbook.
	// The Hall of Masters

	public int initialDamage { get; set; }

	public Ability(string name) : base(name) {
	}

	public Ability (string name, string description, float range, int initialDamage, string initialAnimation, 
		float radius, float dotTickInterval, float dotDuration, float dotDamage, string dotAnimation, 
		float terminalDamage, string terminalAnimation) : base(name) {

		this.initialDamage = initialDamage;
	}

	public abstract bool IsLegal (HeroManager hero);

	public abstract void DoAbility (HeroManager hero);

	public abstract void OnAbilityHitTarget (GameObject sourceAbility);

}

