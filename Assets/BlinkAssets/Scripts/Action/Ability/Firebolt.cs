using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DigitalRuby.PyroParticles;

public class Firebolt : Ability {

	public Firebolt () : base(
		"Firebolt", 
		"Throws a Firebolt at your opponent", 
		30.0f, 50, "initialAnimation", 
		-1.0f, 0, 0, 0, "tickAnimation", 0, "terminalAnimation") {}	

	public override bool IsLegal(HeroManager hero) {
		if (hero.heroCombat.target == null) return false;

		HeroCombat combat = hero.heroCombat;
		Vector3 self = combat.self.characterController.bounds.center;
		Vector3 target = combat.target.characterController.bounds.center;
		Vector3 targetDir = target - self;
		RaycastHit hit;
		Vector3 lineOfSightStart = self + combat.self.transform.forward.normalized * 0.325f;

		float distance = Vector3.Distance (self, target);
		bool inLineOfSight = Physics.Linecast (lineOfSightStart, target, out hit) && hit.transform == combat.target.transform;
		bool facingTarget = Vector3.Angle (combat.self.transform.forward, targetDir) < 90.0f;
		return distance < 30.0f && inLineOfSight && facingTarget;
	}

	//[Command]
	public override void DoAbility(HeroManager hero) {
		HeroCombat combat = hero.heroCombat;
		Vector3 self = combat.self.characterController.bounds.center;
		Vector3 target = combat.target.characterController.bounds.center;
		Vector3 start = self + combat.self.transform.forward.normalized * 0.5f;

		GameObject firebolt = (GameObject) GameObject.Instantiate (Resources.Load("Firebolt"));
		HomingAbility homingAbility = firebolt.GetComponent<HomingAbility>();
		homingAbility.Initialize (start, Quaternion.identity, this, combat.self, combat.target);
		NetworkServer.Spawn (firebolt);
	}

	// prolly need this to be RpcFunction
	public override void OnHit(HeroManager target) {
		target.heroCombat.CmdTakeDamage (this.initialDamage);
	}

}
