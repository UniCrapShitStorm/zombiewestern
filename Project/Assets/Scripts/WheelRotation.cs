using UnityEngine;
using System.Collections;

public class WheelRotation : MonoBehaviour {
	
	private float time;
	
	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		transform.Rotate(0, 0, Mathf.Sin (time));
	}
}
