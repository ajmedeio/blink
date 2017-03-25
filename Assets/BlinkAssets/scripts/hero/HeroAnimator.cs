using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UMA;
using System;

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

	public HashSet<string> movementLayer = new HashSet<string> () {Idle, Run, Jump, BackPedal, Fall, Walk, Stunned, Death};
	public HashSet<string> abilitiesLayer = new HashSet<string> () {AbilitySpell1H};

	public NetAnimator netAnimator;
	public Animator animator;
	public RuntimeAnimatorController runtimeAnimatorController; // initialized inside UnityEditor
	private string lastAnimation = "";

	void Start() {
		
	}

	public void OnUmaAnimatorCreated(Animator umaAnimator) {
		print ("inside HeroAnimator.cs:SetupAnimator(animator)");
		animator = umaAnimator;
		animator.applyRootMotion = false;
		netAnimator = gameObject.AddComponent<NetAnimator> ();
		netAnimator.animator = umaAnimator;

		// We shouldn't even use events, we're only registering ourselves for a single response.
		// We just need a map from NetworkId to OnUmaAnimatorCreated callback. When the animator is created, send the notification
		// to the proper client.
		// TODO Send command to server to remove this from OnUmaAnimatorSetup UMAGeneratorBase.OnUmaAnimatorCreated -= OnUmaAnimatorCreated;
	}

	public void AnimateAbility(HeroManager hero, Transform avatar, string animatorKey) {
		if (animator == null) return;
		animator.Play (animatorKey);
	}

	public void AnimateMovement(HeroManager hero, Transform avatar) {
		string animation = Idle;
		HeroMovement h = hero.heroMovement;

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
			if (animator == null) return;
			animator.SetBool (animation, true);
			FalsifyOthers (animation);
		}
	}

	void FalsifyOthers(string truth) {
		if (animator == null) return;
		foreach (string par in movementLayer) {
			if (truth != par) {
				animator.SetBool (par, false);
			}
		}
	}

}
