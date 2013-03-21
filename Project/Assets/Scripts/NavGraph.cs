using UnityEngine;
using System.Collections.Generic;

public class NavGraph : MonoBehaviour {
	
	public List<GameObject> reachableWaypoints = new List<GameObject>();
	public List<float> distances = new List<float>();
	public GameObject[] waypoints;
	
	// Use this for initialization
	void Start () {
		waypoints = GameObject.FindGameObjectsWithTag("waypoint") as GameObject[];

		foreach (GameObject wp in waypoints) {
			// check if waypoint can see another
			if (!Physics.Linecast (transform.position, wp.transform.position)) {
				// check if it is the waypoint itself
				if (wp.transform.name != this.transform.name) {
					reachableWaypoints.Add(wp);
					distances.Add(calcDistance(transform.position, wp.transform.position));
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	private float calcDistance (Vector3 a, Vector3 b) {
		Vector3 c = a - b;
		return Mathf.Sqrt(Mathf.Pow(c.x, 2)+Mathf.Pow(c.y, 2)+Mathf.Pow(c.z, 2));
	}
}