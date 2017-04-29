using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroCombat : NetworkBehaviour, IObserver {

	[SyncVar] public int health = 1000;

	public int maxHealth = 1000;
	public bool isDead = false;
	public bool isFullHealth = false;

	// The mouse interacts with the world, I need variables to store it's state
	private Camera camera;
	private RaycastHit hit;

	public HeroManager self = null;
	public HeroManager target = null;

	void Start () {
		camera = transform.GetComponentInChildren<Camera> (true);
		self = GetComponent<HeroManager> ();

		health = 1000;
		maxHealth = 1000;
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				HeroManager tmpTarget = hit.transform.GetComponent<HeroManager> ();
				if (tmpTarget != null) {// && target != self
					target = tmpTarget;
					OnTargetChanged ();
				}
			}
		}

		if (transform.position.y < -50) {
			CmdTakeDamage (maxHealth);
		}
	}

	[Command]
	public void CmdTakeDamage(int amount) {
		if (health - amount <= 0) {
			health = 0;
			OnZeroHealth ();
		} else {
			health -= amount;
			OnDamageTaken ();
		}
	}

	[Command]
	public void CmdTakeHealing(int amount) {
		if (health + amount >= maxHealth) {
			health = maxHealth;
			OnFullHealth ();
		} else {
			health += amount;
		}
		OnHealingTaken ();
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

	private void OnTargetChanged() {
		self.heroHud.UpdateTarget ();
	}

	private void OnDamageTaken() {
		self.heroHud.UpdateHealth ();
	}

	private void OnHealingTaken() {
		self.heroHud.UpdateHealth ();
	}

	private void OnZeroHealth() {
		isDead = true;
		self.heroHud.OnDeath ();
	}

	private void OnFullHealth() {
		isFullHealth = true;
	}
}
