using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
	private int health = 30;
	
	void TakeDamage(int damage) {
		health -= damage;
		
		if(health<=0)
			Die();
	}
	
	void Die() {
		// todo: Splatter
		Destroy(gameObject);
		
		print("Zombie is dead.");
	}
}
