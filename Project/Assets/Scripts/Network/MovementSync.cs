using UnityEngine;
using System.Collections;

public class MovementSync : MonoBehaviour {
	private Vector3 position;
	private Quaternion orientation;
	private GameObject FPSController;
	
	public bool localPlayer = false;
	
	void OnNetworkInstantiate(NetworkMessageInfo info) {
		position = transform.position;
		orientation = transform.rotation;
		FPSController = GameObject.Find("First Person Controller(Clone)");
	}
	
	void Update() {
		if(localPlayer) {
			if(FPSController==null) return;
			position = FPSController.transform.position;
			orientation = FPSController.transform.rotation;
			transform.position = FPSController.transform.position;
			transform.rotation = FPSController.transform.rotation;
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		if(stream.isReading) {
			stream.Serialize(ref position);
			stream.Serialize(ref orientation);
			transform.position = position;
			transform.rotation = orientation;
		} else if(stream.isWriting) {
			stream.Serialize(ref position);
			stream.Serialize(ref orientation);
		}
	}
}
