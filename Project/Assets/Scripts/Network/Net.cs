using UnityEngine;
using System.Collections;


public class Net : MonoBehaviour {
	private bool showMenu = false;
	private int port = 27888;
	
	public GameObject firstPersonPrefab;
	public GameObject thirdPersonPrefab;
	
	// Serverside
	private NetworkPlayer[] player;
	private int currPlayer;
	private int maxPlayer = 8;
	
	// Clientside Player View Model
	private Object localPlayer;
	
	// status of local player
	private NetworkConnectionError status;
	private string statusmsg;
	
	void Start () {
		//playerPosition = GameObject.Find("First Person Controller").transform;
	}

	void Update () {
		if(Network.isServer) {}
		if(Network.isClient) {}
		
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(showMenu) {
				showMenu = false;
				Screen.showCursor = false;
				Screen.lockCursor = true;
			} else {
				showMenu=true;
				Screen.showCursor = true;
				Screen.lockCursor = false;
			}
		}
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		statusmsg = player.ipAddress+" connected";
		currPlayer++;
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.DestroyPlayerObjects(player);
		statusmsg = player.ipAddress+" disconnected";
		currPlayer--;
	}
	
	void HostServer() {
		statusmsg = "Hosting...";
		status = Network.InitializeServer(16, port, true);
		
	}
	
	void OnServerInitialized() {
		SpawnPlayer();		
	}
	
	void Connect(string sv) {
		if(Network.isClient || Network.isServer)
			Disconnect();
		
		status = Network.Connect(sv, port);
	}

	void Disconnect() {
		Network.DestroyPlayerObjects(Network.player);
		Network.Disconnect();
		Destroy(localPlayer);
	}
	
	void OnConnectedToServer() {
		SpawnPlayer();
	}
	
	void OnDisconnectedFromServer() {
		
	}
	
	void SpawnPlayer() {
		//todo: spawnlocations
		localPlayer = Instantiate(firstPersonPrefab, transform.position, transform.rotation);
		Object thirdPerson = Network.Instantiate(thirdPersonPrefab, transform.position, transform.rotation, 0);
		Component movement = ((GameObject)thirdPerson).GetComponent("MovementSync");
		((MovementSync)movement).localPlayer = true;
		
		// T_T
		((GameObject)thirdPerson).GetComponentInChildren<SkinnedMeshRenderer>().renderer.enabled = false;
	}
	
	void OnGUI() {
		string ipaddr = "127.0.0.1";
		
		if(!showMenu) return;
		
		if(!Network.isServer)
			if(GUI.Button(new Rect(0,0,100,30), "Host"))
				HostServer();
		
		if(!Network.isClient) {
			GUI.TextArea(new Rect(0,100,100,30),ipaddr);			
			if(GUI.Button(new Rect(0,130,100,30), "Connect")) {
				Connect(ipaddr);
			}
		} else {
			if(GUI.Button(new Rect(0,130,100,30), "Disconnect"))
				Disconnect();
		}
		
		GUI.TextField(new Rect(0,Screen.height-90,100,30), currPlayer+" Player");
		
		if(Network.isServer)
		GUI.TextField(new Rect(0,Screen.height-60,100,30), statusmsg + status.ToString());
	}
}
