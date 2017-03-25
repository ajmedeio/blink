using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroCombat : NetworkBehaviour, IObserver {

	public int health = 1000;
	public int maxHealth = 1000;
	public bool dead = false;

	public HeroManager target = null;
	public HeroManager self = null;

	void Start () {
		maxHealth = 1000;
		health = 1000;
		self = GetComponent<HeroManager> ();
	}

	public void TakeDamage(int amount) {
		if (health - amount <= 0) {
			health = 0;
			OnZeroHealth ();
		} else {
			health -= amount;
		}
	}

	public void TakeHealing(int amount) {
		if (health + amount >= maxHealth) {
			health = maxHealth;
		} else {
			health += amount;
		}
	}

	void IObserver.Notify(IObservable controller, object msg) {
		HashSet<Ability> abilities = msg as HashSet<Ability>;
		if (abilities != null) {
			DoAbilities(abilities);
		}
	}

	private void DoAbilities(HashSet<Ability> abilities) {
		foreach (Ability a in abilities) {
			if (a.IsLegal (self)) {
				a.DoAbility (self);
			}
		}
	}

	private void OnZeroHealth() {
		dead = true;
	}

}
