using System;
using UnityEngine;

public class BasicAttack : Ability {
	
	public BasicAttack () : base("BasicAttack") {
	}

	public override bool IsLegal(HeroManager hero) {
		return true;
	}

	public override void DoAbility(HeroManager hero) {

	}

}
