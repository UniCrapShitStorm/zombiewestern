using UnityEngine;
using System.Collections;

public class ZombieMovement: MonoBehaviour {

	public Transform target;
	
	
	
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public Vector3 moveDirection = Vector3.zero;
	
	public bool isUpstairs = false;
	
	void Start() {
	}
	
    void Update() {
		if (target == null) {
			return;
		}
		
		
		
        CharacterController controller = GetComponent<CharacterController>();
		moveDirection = (target.position - transform.position) * Time.deltaTime;
        moveDirection *= speed;
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
		
		if (target.position.y >= 4.0f && isUpstairs == false) {
			isUpstairs = true;
			int wp = RandomNumber(1,3);
			if (wp == 1)
				target = GameObject.Find("WP1").transform;
			if (wp == 2)
				target = GameObject.Find("WP2").transform;
		}
		
		if (target.position.y < 4.0f) {
			isUpstairs = false;
		}
    }
	
	// Returns a random integer number between min [inclusive] and max [exclusive]
	private int RandomNumber(int min, int max)
	{
		return Random.Range(min, max);
	}
}
	
	/*
	public Transform target;
	public float rotationSpeed = 1.5f;
	public float movingSpeed = 100;
	public float minimalDistanceToObject = 3.0f;
	public float distFromSurfaceToCenter = 0.7f;

	void Awake() {
		animation.wrapMode = WrapMode.Loop;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(target==null)
			return;

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
			animation.CrossFade("run");
		}
		else {
			velocity = Vector3.zero;
			animation.CrossFade("idle");
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
	}*/
