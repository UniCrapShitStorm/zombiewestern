using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject zombie = GameObject.Find("human_animation_legs");
		if(zombie==null)
			return;
		Vector3 z_pos = zombie.transform.position;
		
		GameObject player = GameObject.Find("First Person Controller");
		if(player==null)
			return;
		Vector3 p_pos = player.transform.position;
		Vector3 dir = -z_pos+p_pos;
		if(norm2(dir)<=0.75f)
			return;
		CharacterMotor motor;
        motor = GetComponent<CharacterMotor>();
        motor.inputMoveDirection = transform.rotation * dir/norm2 (dir);
	}
	
	private float norm2(Vector3 v) {
		return Mathf.Sqrt(v.x*v.x+v.y*v.y+v.z*v.z);
	}
}
	