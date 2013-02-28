using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	private float decay = 0.5f; // Abklingzeit
	private float decayTime = 0.0f;
	private int damage = 10;
	private int maxClip = 6;  // Magazingröße
	private int clip;
	private int maxAmmo; // Patronenanzahl
	private int currAmmo = 3*6;	
	public bool drawShells = false;
	private bool isReloading = false;
	private Vector3 defaultPos;
	public Transform weapon = null;
	
	void Start()
	{
		defaultPos = weapon.localPosition;
		Reload();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButton("Fire1"))
			Fire();
		
		if(Input.GetButton("Reload") && !isReloading)
			Reload();
		else
			isReloading = false;
		
		if(Input.GetButton("Fire2"))
			ZoomIn();
		else
			ZoomOut();
	}
	
	void Fire()
	{
		if(currAmmo<=0) return;
		if(decayTime>Time.time) return;
		if(clip==0)
		{
			ClipEmpty();
			return;
		}
		
		RaycastHit[] result = Physics.RaycastAll(transform.position, transform.forward);
		foreach(RaycastHit hit in result)
		{
			print("hit "+hit.distance);
			hit.collider.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
			
			// todo: decal, smoke
		}
		
		if(drawShells)
		{
			GameObject shell = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			shell.transform.position = transform.position;
			shell.transform.RotateAroundLocal(new Vector3(0.0f,0.0f,1.0f),360.0f);
			shell.AddComponent("Rigidbody");
			shell.GetComponent<Rigidbody>().velocity = -transform.right*10.0f;
		}
		
		// todo: muzzleflash, smoke
		
		decayTime = Time.time+decay;
		currAmmo--;
		clip--;
	}
	
	void ClipEmpty()
	{
		print("Plz reload");
	}
	
	void ZoomIn()
	{
		Vector3 temp = new Vector3(-0.1f, 0.0f, 0.1f);
		weapon.localPosition -= (weapon.localPosition-temp)*2.0f*Time.deltaTime;
	}
	
	void ZoomOut()
	{
		weapon.localPosition -= (weapon.localPosition-defaultPos)*2.0f*Time.deltaTime;
	}
	
	void Reload()
	{
		isReloading = true;
		if(currAmmo==0) return;
		
		print("Reloading");
		currAmmo-=clip; // Restliche Patronen verwerfen
		clip = (currAmmo>maxClip)?maxClip:currAmmo;
		decayTime = Time.time+decay;
	}
	
	void OnGUI()
	{
		GUI.Box(new Rect(20,Screen.height-20,40,20), clip + "("+Mathf.Floor((currAmmo-clip)/maxClip)+")");
	}
}
