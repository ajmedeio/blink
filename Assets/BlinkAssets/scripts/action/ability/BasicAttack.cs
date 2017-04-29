using System;
using UnityEngine;
using UnityEngine.Networking;

public class BasicAttack : Ability {
	
	public BasicAttack () : base("BasicAttack") {
	}

	public override bool IsLegal(HeroManager hero) {
		return true;
	}

	//[Command]
	public override void DoAbility(HeroManager hero) {

	}

	public override void OnHit(HeroManager target) {

	}
}
