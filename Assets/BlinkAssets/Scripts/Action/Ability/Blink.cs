using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Blink : Ability {

	public Blink () : base(
		"Blink", 
		"Blinks the player forward", 
		30.0f, 500, "initialAnimation", 
		-1.0f, 0, 0, 0, "tickAnimation", 0, "terminalAnimation") {}			

	public override bool IsLegal(HeroManager hero) {
		return true;
	}

	public override void DoAbility(HeroManager hero) {
		// Is view blocked?
		RaycastHit hit;
		Vector3 start = hero.characterController.bounds.center;
		Transform avatar = hero.transform.FindChild ("AvatarContainer").transform;
		Vector3 translation = avatar.forward.normalized * 2.5f;

		// Cast the line to check if camera is in front of an object, shorten distance if so:
		int heroesLayer = LayerMask.NameToLayer("Heroes");
		int notHeroesLayer = ~heroesLayer;
		if (Physics.Linecast (start, start + translation, out hit, notHeroesLayer)) {
			translation = (start + translation) - hit.point;
		}

		hero.transform.position += translation;
	}


	public override void OnAbilityHitTarget (HeroManager target) {
		return;
	}
}
