using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HomingAbility : NetworkBehaviour {

	Ability ability;
	HeroManager sender;
	HeroManager target;
	float speed = 25.0f;

	bool initialized = false;

	// Use this for initialization
	void Start () {
		// just wait for the insantiator to initialize everything
	}

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
			ability.OnHit (target);
			GameObject.Destroy (gameObject);
		}
	}
}
