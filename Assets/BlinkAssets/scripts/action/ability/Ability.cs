using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : Action {

	public Ability(string name) : base(name) {
	}

	public Ability (string name, string description, float range, float initialDamage, string initialAnimation, 
		float radius, float dotTickInterval, float dotDuration, float dotDamage, string dotAnimation, 
		float terminalDamage, string terminalAnimation) : base(name) {

	}

	public abstract bool IsLegal (Hero hero);

	public abstract void DoAbility (Hero hero);

}

