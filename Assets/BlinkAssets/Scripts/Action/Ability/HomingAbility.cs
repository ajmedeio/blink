using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HomingAbility : NetworkBehaviour {

	public Ability ability;
    public HeroManager sender;
    public HeroManager target;
    public float speed = 25.0f;

    public bool initialized = false;

	public void Initialize(Vector3 position, Quaternion rotation, Ability ability, HeroManager sender, HeroManager target) {
		transform.position = position;
		transform.rotation = rotation;
		this.ability = ability;
		this.sender = sender;
		this.target = target;
		initialized = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!initialized) return;

		// Move towards target until collision
		Vector3 targetCenter = target.characterController.bounds.center;
		Vector3 targetDir = targetCenter - transform.position;
		transform.position += targetDir.normalized * speed * Time.deltaTime;

		if (targetDir.magnitude < target.characterController.bounds.extents.z) {
			ability.OnAbilityHitTarget (this.gameObject);
			NetworkServer.Destroy (this.gameObject);
		}
	}
}
