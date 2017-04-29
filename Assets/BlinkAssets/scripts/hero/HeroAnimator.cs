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

	public Animator animator;

	[SyncVar] private string lastMovementAnim = "";
	[SyncVar] private string lastAbilityAnimation = "";

	[SyncVar(hook="IdleChanged")] public bool idle = false;
	[SyncVar(hook="RunChanged")] public bool run = false;
	[SyncVar(hook="JumpChanged")] public bool jump = false;
	[SyncVar(hook="BackPedalChanged")] public bool backPedal = false;
	[SyncVar(hook="FallChanged")] public bool fall = false;
	[SyncVar(hook="WalkChanged")] public bool walk = false;
	[SyncVar(hook="StunnedChanged")] public bool stunned = false;
	[SyncVar(hook="DeathChanged")] public bool death = false;
	[SyncVar(hook="AbilitySpell1HChanged")] public bool abilitySpell1H = false;

	public void IdleChanged(bool value) { idle = value; animator.SetBool (Idle, value); }
	public void RunChanged(bool value) { run = value; animator.SetBool (Run, value); }
	public void JumpChanged(bool value) { jump = value; animator.SetBool (Jump, value); }
	public void BackPedalChanged(bool value) { backPedal = value; animator.SetBool (BackPedal, value); }
	public void FallChanged(bool value) { fall = value; animator.SetBool (Fall, value); }
	public void WalkChanged(bool value) { walk = value; animator.SetBool (Walk, value); }
	public void StunnedChanged(bool value) { stunned = value; animator.SetBool (Stunned, value); }
	public void DeathChanged(bool value) { death = value; animator.SetBool (Death, value); }
	public void AbilitySpell1HChanged(bool value) { abilitySpell1H = value; animator.SetBool (AbilitySpell1H, value); }

	private Dictionary<string, Action<bool>> movementLayer;
	private Dictionary<string, Action<bool>> abilitiesLayer;

	void Start() {
		movementLayer = new Dictionary<string, Action<bool>> 
		{
			{Idle, IdleChanged},
			{Run, RunChanged},
			{Jump, JumpChanged},
			{BackPedal, BackPedalChanged},
			{Fall, FallChanged},
			{Walk, WalkChanged},
			{Stunned, StunnedChanged},
			{Death, DeathChanged}
		};

		abilitiesLayer = new Dictionary<string, Action<bool>>
		{
			{AbilitySpell1H, AbilitySpell1HChanged}
		};
	}

	public void OnAvatarCreated() {
		animator = GetComponentInChildren<Animator> ();
		if (animator != null) animator.applyRootMotion = false;
	}


	public void AnimateAbility(HeroManager hero, Transform avatar, string animationName) {
		//animator.Stop ();
		//animator.Play (animationName);
		//lastAbilityAnimation = animationName;
	}

	public void AnimateMovement(HeroManager hero, Transform avatar) {
		if (animator == null) {
			OnAvatarCreated ();
			return;
		}
		
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

			// rotate uma to face direction of motion
			if (h.zMotion < 0)
				avatar.rotation = Quaternion.Slerp (avatar.rotation, Quaternion.LookRotation (-h.xzMovement), 0.85f);
			else
				avatar.rotation = Quaternion.Slerp (avatar.rotation, Quaternion.LookRotation (h.xzMovement), 0.85f);
			
		} else {
			// rotate uma back to forward position
			avatar.rotation = Quaternion.Slerp (avatar.rotation, Quaternion.LookRotation (h.transform.forward), 1f);
		}

		if (!h.isGrounded) {
			if (h.yMovement.y > 0) animation = Fall;
			else animation = Fall;
		}

		if (hero.heroCombat.isDead) {
			animation = Death;
		}

		if (lastMovementAnim != animation) CmdStartAnimation (animation);
		lastMovementAnim = animation;
	}

	[Command]
	public void CmdStartAnimation(string animation) {
		movementLayer [animation] (true);
		FalsifyOthers (animation);
	}

	void FalsifyOthers(string truth) {
		foreach (KeyValuePair<string, Action<bool>> kv in movementLayer) {
			if (truth != kv.Key) {
				movementLayer [kv.Key] (false);
			}
		}
	}

}
