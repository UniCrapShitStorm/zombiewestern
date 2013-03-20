using UnityEngine;
using System.Collections.Generic;

public class NavGraph : MonoBehaviour {
	
	public List<string> reachableWaypoints = new List<string>();
	public GameObject[] waypoints;
	
	private RaycastHit hit;
	private Vector3 rayDirection;
	
	// Use this for initialization
	void Start () {
		waypoints = GameObject.FindGameObjectsWithTag("waypoint") as GameObject[];

		foreach (GameObject wp_outer in waypoints) {
			foreach (GameObject wp_inner in waypoints) {
				rayDirection = wp_outer.transform.position - wp_inner.transform.position;
				if (Physics.Linecast (wp_inner.transform.position, wp_outer.transform.position, out hit)) {
					if (hit.transform.tag == "waypoint") {
						string wp_inner_name = wp_inner.transform.name.ToString().Substring(2);
						string wp_outer_name = wp_outer.transform.name.ToString().Substring(2);
						reachableWaypoints.Add(wp_outer_name + " -> " + wp_inner_name);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
