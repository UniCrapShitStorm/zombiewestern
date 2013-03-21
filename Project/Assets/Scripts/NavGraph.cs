using UnityEngine;
using System.Collections.Generic;

public class NavGraph : MonoBehaviour {
	
	public List<GameObject> reachableWaypoints = new List<GameObject>();
	public GameObject[] waypoints;
	
	// Use this for initialization
	void Start () {
		waypoints = GameObject.FindGameObjectsWithTag("waypoint") as GameObject[];

		foreach (GameObject wp in waypoints) {
			if (!Physics.Linecast (transform.position, wp.transform.position)) {
				reachableWaypoints.Add(wp);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}