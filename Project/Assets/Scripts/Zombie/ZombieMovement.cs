using UnityEngine;
using System.Collections;

public class ZombieMovement: MonoBehaviour {
	
	public Transform target;
	public float rotationSpeed = 1.5f;
	public float movingSpeed = 100;
	public float minimalDistanceToObject = 3.0f;
	public float distFromSurfaceToCenter = 0.7f;

	void Awake() {
		//animation.wrapMode = WrapMode.Loop;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		rotateTo(target.position);
		walk (target.position);
		surfaceDetection();
	}
	
	void rotateTo(Vector3 targetPosition) {
		// transform.LookAt(target);
		
		Quaternion destRotation;
		Vector3 relativePos;
		
		Vector3 tmpTargetPosition = targetPosition;
		Vector3 tmpTransformPosition = transform.position;
		
		tmpTargetPosition.y = 0;
		tmpTransformPosition.y = 0;
		relativePos = tmpTargetPosition - tmpTransformPosition;

		destRotation = Quaternion.LookRotation(relativePos);
		transform.rotation = Quaternion.Slerp(transform.rotation, destRotation, rotationSpeed * Time.deltaTime);
	}
	
	void walk (Vector3 targetPosition) {
		Vector3 velocity;
		Vector3 moveDirection = transform.TransformDirection(Vector3.forward);
		Vector3 delta = targetPosition - transform.position;
		
		if (delta.magnitude > minimalDistanceToObject) {
			velocity = moveDirection.normalized * movingSpeed * Time.deltaTime;
			//animation.CrossFade("run");
		}
		else {
			velocity = Vector3.zero;
			//animation.CrossFade("idle");
		}
		rigidbody.velocity = velocity;
	}
	
	void surfaceDetection() {
		float surfaceSpeedDown = 100;
		RaycastHit hit;
		
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, distFromSurfaceToCenter)) {
			if (hit.distance < distFromSurfaceToCenter - 0.2) {
				rigidbody.velocity += (Vector3.up * surfaceSpeedDown * Time.deltaTime);
			}
		}
		else {
			rigidbody.velocity += (-Vector3.up * surfaceSpeedDown * Time.deltaTime); 
		}
		
		Debug.DrawRay(transform.position, -Vector3.up * distFromSurfaceToCenter, Color.green);
	}
}
