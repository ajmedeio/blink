using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamera : MonoBehaviour {

	public float targetHeight = 0.5f;
	public float distance = 2.8f;
	public int maxDistance = 10;
	public float minDistance = 1.0f;
	public float xSpeed = 350.0f;
	public float ySpeed = 120.0f;
	public int yMinLimit = -40;
	public int yMaxLimit = 80;
	public int zoomRate = 40;
	public float rotationDampening = 3.0f;
	public float x = 0.0f;
	public float y = 0.0f;

	public Hero hero;
	private Transform target;

	// Use this for initialization
	void Start () {
		hero = GameObject.FindWithTag ("Hero").GetComponent<Hero> ();
		target = hero.transform;
		x = transform.eulerAngles.y;
		y = transform.eulerAngles.x;

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>() != null)
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	// Update is called once per frame
	void LateUpdate () {
		
		// If either mouse buttons are down, let them govern camera position 
		if (hero.changeCameraAngle || hero.changeHeroAngle) {
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			// otherwise, ease behind the target if any of the directional keys are pressed 
		} else if (hero.xMotion != 0 || hero.zMotion != 0) {
			//float targetRotationAngle = target.eulerAngles.y;
			//float currentRotationAngle = transform.eulerAngles.y;
			//x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
		}

		distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
		y = ClampAngle(y, yMinLimit, yMaxLimit);

		// Rotate camera
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		transform.rotation = rotation;

		// Position camera
		Vector3 minTargetHeight = new Vector3 (0, -targetHeight, 0);
		Vector3 position = target.position - (rotation * Vector3.forward * distance + minTargetHeight);
		transform.position = position;

		// Is view blocked?
		RaycastHit hit;
		Vector3 trueTargetPosition = target.transform.position - minTargetHeight;

		// Cast the line to check if camera is in front of an object, shorten distance if so:
		if (Physics.Linecast (trueTargetPosition, transform.position, out hit)) {
			if (hit.collider != hero.characterController) {
				float tempDistance = Vector3.Distance (trueTargetPosition, hit.point) - 0.28f;

				// Finally, reposition the camera:
				position = target.position - (rotation * Vector3.forward * tempDistance + minTargetHeight);
				transform.position = position;
			}
		}

		// reset actions to initial conditions so they don't "persist" into the next frame
		hero.resetActions ();
	}

	public float ClampAngle(float angle, float min, float max) { 
		if (angle < -360) angle += 360;
		else if (angle > 360) angle -= 360;

		return Mathf.Clamp (angle, min, max);
	}
}