using UnityEngine;
using System.Collections;

public class MovementSync : MonoBehaviour {
	private Vector3 position;
	private GameObject FPSController;
	
	public bool localPlayer = false;
	
	void OnNetworkInstantiate(NetworkMessageInfo info) {
		print ("nettwork instantiate");
		position = transform.position;
		FPSController = GameObject.Find("First Person Controller(Clone)");
	}
	
	void Update() {
		if(localPlayer) {
			if(FPSController==null) return;
			position = FPSController.transform.position;
			transform.position = FPSController.transform.position;
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		print ("sync");
		if(stream.isReading) {
			stream.Serialize(ref position);
			transform.position = position;
		} else if(stream.isWriting) {
			stream.Serialize(ref position);
		}
	}
}
