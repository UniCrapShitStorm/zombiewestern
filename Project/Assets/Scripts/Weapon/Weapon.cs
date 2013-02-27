using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	private float decay = 0.5f;
	private float decayTime = 0.0f;
	private int maxAmmo;
	private int currAmmo = 30;
	
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("mouse 0"))
			Fire();
	}
	
	void Fire() {
		if(currAmmo<=0) return;
		
		if(decayTime>Time.time)
			return;
		
		RaycastHit[] result = Physics.RaycastAll(transform.position, transform.forward);
		foreach(RaycastHit hit in result) {
			print("hit "+hit.distance);
		}
		
		decayTime = Time.time+decay;
		currAmmo--;
	}
}
