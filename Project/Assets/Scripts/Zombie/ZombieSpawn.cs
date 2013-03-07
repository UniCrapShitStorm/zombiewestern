using UnityEngine;
using System.Collections;

public class ZombieSpawn : MonoBehaviour {
	
	public GameObject respawnPrefab;
	public int NumberOfZombies;
	private GameObject[] respawnPoints;
	
	// Use this for initialization
	void Start () {
		respawnPoints = GameObject.FindGameObjectsWithTag("ZombieSpawn");
        int NumberOfSpawnPoints = respawnPoints.Length;
		
		for (int i = 0; i < NumberOfZombies; i++) {
			int spawnPoint = RandomNumber(0, NumberOfSpawnPoints);
			
			Vector3 rand = new Vector3(RandomNumber(-4,4), 1, RandomNumber(-4,4));
			
			Instantiate(respawnPrefab, respawnPoints[spawnPoint].transform.position + rand, respawnPoints[spawnPoint].transform.rotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Returns a random integer number between min [inclusive] and max [exclusive]
	private int RandomNumber(int min, int max)
	{
		
		return Random.Range (min, max);
	}
}
