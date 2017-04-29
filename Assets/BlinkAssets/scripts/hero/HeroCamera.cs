using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroCamera : NetworkBehaviour {

	public float targetHeight = 1.35f;
	public float distance = 6.0f;
	public int maxDistance = 10;
	public float minDistance = 1.0f;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public int yMinLimit = -40;
	public int yMaxLimit = 80;
	public int zoomRate = 40;
	public float rotationDampening = 3.0f;
	public float x = 0.0f;
	public float y = 0.0f;

	private HeroMovement movement;
	private Transform cameraTransform;

	// Use this for initialization
	void Start () {
		cameraTransform = GetComponentInChildren<Camera>(true).transform;
		if (isLocalPlayer) cameraTransform.gameObject.SetActive (true);
		movement = GetComponent<HeroMovement> ();
		x = transform.eulerAngles.y;
		y = transform.eulerAngles.x;

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>() != null)
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	public void UpdateHeroCamera () {
		if (!hasAuthority) return;

		// If either mouse buttons are down, let them govern camera position 
		if (movement.changeCameraAngle || movement.changeHeroAngle) {
			x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
			y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
			// otherwise, ease behind the target if any of the directional keys are pressed 
		} else if (movement.xMotion != 0 || movement.zMotion != 0) {
			//float targetRotationAngle = target.eulerAngles.y;
			//float currentRotationAngle = transform.eulerAngles.y;
			//x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
		}

		distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
		y = ClampAngle(y, yMinLimit, yMaxLimit);

		// Rotate camera
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		cameraTransform.rotation = rotation;

		// Position camera
		Vector3 minTargetHeight = new Vector3 (0, -targetHeight, 0);
		Vector3 position = transform.position - (rotation * Vector3.forward * distance + minTargetHeight);
		cameraTransform.position = position;

		// Is view blocked?
		RaycastHit hit;
		Vector3 trueTargetPosition = transform.position - minTargetHeight;

		// Cast the line to check if camera is in front of an object, shorten distance if so:
		if (Physics.Linecast (trueTargetPosition, cameraTransform.position, out hit)) {
			if (hit.collider != movement.characterController) {
				float tempDistance = Vector3.Distance (trueTargetPosition, hit.point) - 0.28f;

				// Finally, reposition the camera:
				position = transform.position - (rotation * Vector3.forward * tempDistance + minTargetHeight);
				cameraTransform.position = position;
			}
		}
	}

	public float ClampAngle(float angle, float min, float max) { 
		if (angle < -360) angle += 360;
		else if (angle > 360) angle -= 360;

		return Mathf.Clamp (angle, min, max);
	}
}