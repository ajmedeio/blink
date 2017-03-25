using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		Vector3 start = hero.transform.position + new Vector3(0.0f, -0.65f, 0.0f);
		Vector3 translation = new Vector3(0.0f, 0.0f, 2.5f);
		translation = hero.transform.TransformDirection (translation);

		// Cast the line to check if camera is in front of an object, shorten distance if so:
		if (Physics.Linecast (start, start + translation, out hit)) {
			// if the cast hits then it traveled the length of the radius of the capsulecollider
			// which needs to be subtracted out with the 0.45f
			// - 0.45f
			translation = start + translation - hit.point;
		}

		hero.transform.position += translation;
		hero.heroAvatar.AnimateAbility (hero, HeroAnimator.AbilitySpell1H);
	}
}
