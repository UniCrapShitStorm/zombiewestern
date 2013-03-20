using UnityEngine;
using System.Collections.Generic;

public class NavGraph : MonoBehaviour {
	
	public List<string> reachableWaypoints = new List<string>();
	
	// Use this for initialization
	void Start () {
		Vector3 direction = Vector3.forward;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.forward, out hit)) {
			if (hit.transform.tag == "waypoint") {
				reachableWaypoints.Add(hit.collider.gameObject.name);
			}
		}			
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
