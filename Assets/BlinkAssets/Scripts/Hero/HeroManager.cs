using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroManager : NetworkBehaviour {

	public HeroAvatar heroAvatar;
	public HeroMovement heroMovement;

	// Use this for initialization
	void Start () {
		heroAvatar = GetComponent<HeroAvatar> ();
		heroMovement = GetComponent<HeroMovement> ();
	}

}
