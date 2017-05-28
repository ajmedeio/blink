using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroCombat : NetworkBehaviour, IObserver {

	// state variables
	[SyncVar(hook="HealthChanged")] public int health = 1000;
	[SyncVar(hook="MaxHealthChanged")] public int maxHealth = 1000;
	[SyncVar(hook="AbilityChanged")] public string ability = "";
	public HeroManager self = null;
	public HeroManager target = null;
    private NetworkStartPosition[] spawnPoints;

	// The mouse interacts with the world, I need variables to store it's state
	private Camera camera;
	private RaycastHit hit;

	// There are a lot of events related to health changes and abilities done which need to be watched
	// by objects such as the HeroHud and the HeroAnimator
	public delegate void HeroCombatValueChangedEventHandler<T>(T oldVal, T newVal);
    public delegate void HeroCombatEventHandler<T>(T param);
	public event HeroCombatValueChangedEventHandler<HeroManager> OnTargetChanged;
	public event HeroCombatValueChangedEventHandler<int> OnHealthChanged;
	public event HeroCombatValueChangedEventHandler<int> OnMaxHealthChanged;
	public event HeroCombatValueChangedEventHandler<Ability> OnAbilityChanged;
    public event HeroCombatEventHandler<GameObject> OnDeath;

	void Start () {
        if (isLocalPlayer) { 
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }

		camera = transform.GetComponentInChildren<Camera> (true);
		self = GetComponent<HeroManager> ();

		health = 1000;
		maxHealth = 1000;
	}

	void Update() {
        if (!hasAuthority) return;
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				HeroManager tmpTarget = hit.transform.GetComponent<HeroManager> ();
				if (tmpTarget != null) {// && target != self
					HeroManager oldTarget = target;
					target = tmpTarget;
					if (OnTargetChanged != null) OnTargetChanged (oldTarget, target);
				}
			}
		}

		if (AbilityMap.underMap.IsLegal()) {
			CmdChangeHealthBy (AbilityMap.underMap, -maxHealth);
		}
	}

	public void HealthChanged(int newHealth) {
		int oldHealth = health;
		health = newHealth;
		if (OnHealthChanged != null) OnHealthChanged (oldHealth, health);
	}

	public void MaxHealthChanged(int newMaxHealth) {
		int oldMaxHealth = maxHealth;
		maxHealth = newMaxHealth;
		if (OnMaxHealthChanged != null) OnMaxHealthChanged (oldMaxHealth, maxHealth);
	}

	public void AbilityChanged(string newAbility) {
		if (!String.Equals(newAbility, "", StringComparison.Ordinal)) {
			Ability oldAbility;
			Ability a;
			AbilityMap.masterAbilityMap.TryGetValue (ability, out oldAbility);
			AbilityMap.masterAbilityMap.TryGetValue (newAbility, out a);
			if (a != null && a.IsLegal (self)) a.DoAbility (self);
			if (OnAbilityChanged != null) OnAbilityChanged (oldAbility, a);

			// need to reset ability, otherwise doing the same ability twice in a row won't trigger
			// the SyncVar's dirtybit
            // CmdDoAbilityByName(), this has to be a command, otherwise it won't run on the server
            // which means it won't be synchronized
			CmdDoAbilityByName("");
		}
	}

	[TargetRpc]
	public void TargetHit(NetworkConnection target, GameObject sourceAbility) {
        HomingAbility ability = sourceAbility.GetComponent<HomingAbility>();
        CmdChangeHealthBy(sourceAbility, -ability.ability.initialDamage);
	}

	[Command]
	public void CmdAbilityHitMe(GameObject sourceAbility) {
        
		TargetHit (target.GetComponent<NetworkIdentity>().connectionToClient, );
	}

	[Command]
	public void CmdSpawnHomingAbility(NetVector3 vector, NetQuaternion quaternion, string spawnableResource, GameObject self, GameObject target) {
		GameObject firebolt = (GameObject) GameObject.Instantiate (Resources.Load(spawnableResource));
		Ability ability;
		AbilityMap.masterAbilityMap.TryGetValue (spawnableResource, out ability);
		HomingAbility homingAbility = firebolt.GetComponent<HomingAbility>();
		homingAbility.Initialize (vector.vector, quaternion.quaternion, ability, self.GetComponent<HeroManager>(), target.GetComponent<HeroManager>());
		NetworkServer.Spawn (firebolt);
	}

	[Command]
	public void CmdChangeHealthBy(GameObject sourceAbility, int amount) {
		int oldHealth = health;
		if (health + amount <= 0) {
			health = 0;
            if (OnDeath != null) OnDeath(sourceAbility);
		} else if (health + amount >= maxHealth) {
			health = maxHealth;
		} else {
			health += amount;
		}
		// implicitly sets all client healths to correct values because health is SyncVar
	}

	[Command]
	private void CmdDoAbilityByName(string abilityName) {
		//string oldAbility = ability;
		ability = abilityName;
	}

    public void Resurrect() {
        Vector3 spawnPoint = Vector3.zero;
        if (spawnPoints != null && spawnPoints.Length > 0) {
            spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
        }
        transform.position = spawnPoint;
        CmdChangeHealthBy(maxHealth);
    }

	// msgs are usually sent by the Controller
	void IObserver.Notify(IObservable controller, object msg) {
		if (health == 0) return;

		HashSet<Ability> abilities = msg as HashSet<Ability>;
		if (abilities != null) {
			foreach (Ability a in abilities) {
				CmdDoAbilityByName (a.name);
			}
		}
	}

}
