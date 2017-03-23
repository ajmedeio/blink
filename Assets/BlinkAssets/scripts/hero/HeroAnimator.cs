using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroAnimator : NetworkBehaviour {

	public static readonly string Idle = "Idle";
	public static readonly string Run = "Run";
	public static readonly string Jump = "Jump";
	public static readonly string BackPedal = "BackPedal";
	public static readonly string Fall = "Fall";
	public static readonly string Walk = "Walk";
	public static readonly string Stunned = "Stunned";
	public static readonly string Death = "Death";
	public static readonly string AbilitySpell1H = "AbilitySpell1H";

	public HashSet<string> movementLayer;
	public HashSet<string> abilitiesLayer;

	public Animator animator;
	public RuntimeAnimatorController runtimeAnimatorController;
	private string lastAnimation = "";

	void Start() {
		movementLayer = new HashSet<string> ();
		abilitiesLayer = new HashSet<string> ();

		movementLayer.Add (Idle);
		movementLayer.Add (Run);
		movementLayer.Add (Jump);
		movementLayer.Add (BackPedal);
		movementLayer.Add (Fall);
		movementLayer.Add (Walk);
		movementLayer.Add (Stunned);
		movementLayer.Add (Death);

		abilitiesLayer.Add (AbilitySpell1H);
	}

	public void AnimateAbility(Hero h, Transform avatar, string animatorKey) {
		animator.Play (animatorKey);
	}

	public void AnimateMovement(Hero h, Transform avatar) {
		string animation = Idle;

		if (h.xzMovement != Vector3.zero) {
			if (h.xMove) animation = Run;
			if (h.zMove) {
				if (h.zMotion > 0)
					if (h.isWalking) animation = Walk;
					else animation = Run;
				else animation = BackPedal;
			}

			if (h.zMotion < 0) avatar.rotation = Quaternion.Slerp (avatar.rotation, Quaternion.LookRotation (new Vector3(-h.xzMovement.x, 0.0f, -h.xzMovement.z)), 0.65f);
			else avatar.rotation = Quaternion.Slerp (avatar.rotation, Quaternion.LookRotation (new Vector3(h.xzMovement.x, 0.0f, h.xzMovement.z)), 0.65f);
		} else {
			avatar.rotation = Quaternion.Slerp (avatar.rotation, Quaternion.LookRotation (h.transform.forward), 1f);
		}

		if (!h.isGrounded) {
			if (h.yMovement.y > 0) animation = Fall;
			else animation = Fall;
		}

		if (lastAnimation != animation) {
			animator.SetBool (animation, true);
			FalsifyOthers (animation);
		}
	}

	void FalsifyOthers(string truth) {
		foreach (string par in movementLayer) {
			if (truth != par) {
				animator.SetBool (par, false);
			}
		}
	}

}
