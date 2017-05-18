using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroManager : NetworkBehaviour {

	public HeroAvatar heroAvatar;
	public HeroMovement heroMovement;
	public HeroCombat heroCombat;
	public HeroHud heroHud;
	public CharacterController characterController;
    public Camera camera;


	// Use this for initialization
	void Start () {
		heroAvatar = GetComponent<HeroAvatar> ();
		heroMovement = GetComponent<HeroMovement> ();
		heroCombat = GetComponent<HeroCombat> ();
		heroHud = GetComponent<HeroHud> ();
		characterController = GetComponent<CharacterController> ();
        camera = GetComponentInChildren<Camera>(true);
	}

}
