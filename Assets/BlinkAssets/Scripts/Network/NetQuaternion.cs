using UnityEngine;

public struct NetQuaternion {
	public Quaternion quaternion;

	public NetQuaternion(Quaternion quaternion) {
		this.quaternion = quaternion;
	}
}
