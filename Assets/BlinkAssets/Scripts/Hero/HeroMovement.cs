using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroMovement : NetworkBehaviour, IObserver {

	// Movement Variables
	public bool isWalking = false;
	public bool isGrounded = false;
	public Vector3 gravity = new Vector3 (0.0f, -1.0f, 0.0f);
	public Vector3 jump = new Vector3 (0.0f, 25.0f, 0.0f);
	public float maxYSpeed = 50.0f;

	public float rotateSpeed = 150.0f;
	public float runSpeed = 5.0f;
	public float backPedalSpeed = 3.0f;
	public float walkSpeed = 1.7f;
	public float inAirSpeed = 3.0f;
	public float xzMoveSpeed = 5.0f;

	public Vector3 yMovement = Vector3.zero;
	public Vector3 xzMovement = Vector3.zero;
	public Vector3 lastXzMove = Vector3.zero;

	// Complex objects which a hero posseses
	private Transform heroCameraTransform;
	private HeroAvatar heroAvatar;
	private HeroManager heroManager;
	public CharacterController characterController;

	// per frame state variables, these values will change pretty much every frame 
	// and indicate which actions were sent to the hero in Notify.
	public bool minusX;
	public bool minusY;
	public bool minusZ;
	public bool plusX;
	public bool plusY;
	public bool plusZ;
	public bool minusYRotate;
	public bool plusYRotate;
	public bool stand;
	public bool xMove;
	public bool zMove;
	public bool yRotate;
	public bool changeCameraAngle;
	public bool changeHeroAngle;
	public short xMotion; 					// left = -1, right = 1, left && right = 0
	public short zMotion; 					// back = -1, forward = 1, back && forward = 0
	public short yRotation;					// left = -1, right = 1, left && right = 0

	private delegate void VoidFunc();
	private Dictionary<MovementAction, VoidFunc> heroActionMap;

	void Start () {
		resetMovement ();

		heroAvatar = GetComponent<HeroAvatar> ();
		characterController = GetComponent<CharacterController>();
		heroCameraTransform = GetComponentInChildren<Camera>(true).transform;
		heroManager = GetComponent<HeroManager> ();

		heroActionMap = new Dictionary<MovementAction, VoidFunc> {
			{MovementAction.MinusX, MinusX},
			{MovementAction.MinusY, MinusY},
			{MovementAction.MinusZ, MinusZ},
			{MovementAction.PlusX, PlusX},
			{MovementAction.PlusY, PlusY},
			{MovementAction.PlusZ, PlusZ},
			{MovementAction.MinusYRotate, MinusYRotate},
			{MovementAction.PlusYRotate, PlusYRotate},
			{MovementAction.ChangeCameraAngle, ChangeCameraAngle},
			{MovementAction.ChangeHeroAngle, ChangeHeroAngle},
			{MovementAction.ToggleRunWalk, ToggleRunWalk}
		};
	}

	public void resetMovement() {
		minusX = false;
		minusY = false;
		minusZ = false;
		plusX = false;
		plusY = false;
		plusZ = false;
		minusYRotate = false;
		plusYRotate = false;

		stand = false;
		xMove = false;
		zMove = false;
		yRotate = false;

		changeCameraAngle = false;
		changeHeroAngle = false;
		xMotion = 0;
		zMotion = 0;
		yRotation = 0;
	}

	// msg is usually an Action object
	void IObserver.Notify(IObservable controller, object msg) {
		HashSet<MovementAction> actions = msg as HashSet<MovementAction>;
		if (actions != null) {
			foreach (MovementAction ma in actions) {
				heroActionMap [ma] ();
			}
			DoMovement ();
		}
	}

	private void DoMovement() {
		// figure out the direction we're going to move including rotations
		xMotion = 0;
		if (minusX) xMotion -= 1;
		if (plusX) xMotion += 1;
		zMotion = 0;
		if (minusZ) zMotion -= 1;
		if (plusZ || (changeHeroAngle && changeCameraAngle)) zMotion += 1;
		yRotation = 0;
		if (minusYRotate) yRotation -= 1;
		if (plusYRotate) yRotation += 1;

		if (yRotation != 0 && changeHeroAngle) {
			xMotion = yRotation;
			yRotation = 0;
		}

		xMove = xMotion != 0;
		yRotate = yRotation != 0;
		zMove = zMotion != 0;

		// Create a vector representing our movement
		xzMovement = new Vector3(xMotion, 0, zMotion);

		if (zMotion < 0) xzMoveSpeed = backPedalSpeed;
		else if (isWalking) xzMoveSpeed = walkSpeed;
		else xzMoveSpeed = runSpeed;

		// if the character is in the air, only apply movement in a small way
		if (!isGrounded) {
			//xzMoveSpeed = inAirSpeed;
			if (yMovement.y > -maxYSpeed) yMovement += gravity;
		} else {
			yMovement = gravity;
		}
			
		if (isGrounded && plusY) yMovement += jump;

		if (changeHeroAngle) transform.rotation = Quaternion.Euler (0, heroCameraTransform.eulerAngles.y, 0);
		else transform.Rotate (0, yRotation * rotateSpeed * Time.deltaTime, 0);

		//Move the avatar
		xzMovement = transform.TransformDirection (xzMovement);
		CollisionFlags collisionFlags = characterController.Move (xzMovement.normalized * xzMoveSpeed * Time.deltaTime + yMovement * Time.deltaTime);
		isGrounded = (collisionFlags & CollisionFlags.Below) != 0;

		// perform animations
		heroAvatar.AnimateMovement(heroManager);

		lastXzMove = xzMovement;
	}

	public void Stand() {
		stand = true;
	}

	public void MinusX() {
		minusX = true;
	}

	public void MinusY() {
		minusY = true;
	}

	public void MinusZ() {
		minusZ = true;
	}

	public void PlusX() {
		plusX = true;
	}

	public void PlusY() {
		plusY = true;
	}

	public void PlusZ() {
		plusZ = true;
	}

	public void MinusYRotate() {
		minusYRotate = true;
	}

	public void PlusYRotate() {
		plusYRotate = true;
	}

	public void ChangeHeroAngle() {
		changeHeroAngle = true;
	}

	public void ChangeCameraAngle() {
		changeCameraAngle = true;
	}

	public void ToggleRunWalk() {
		isWalking = !isWalking;
	}
}
