using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ChatMessage {
	public float timestamp;
	public string player;
	public string message;
};

public class Net : MonoBehaviour {
	private int showMenu = 1;
	private int lastMenu = 0;
	public const int MENU_MAIN 			= 1;
	public const int MENU_CONNECT 		= (1<<1);
	public const int MENU_CONNECTED 	= (1<<2);
	public const int MENU_SETTING 		= (1<<3);
	public const int MENU_CHATLATEST	= (1<<4);
	public const int MENU_CHATBOX 		= (1<<5);
	public const int MENU_INGAME = MENU_CHATLATEST;
	public const float CHAT_DISPLAYTIME	= 7.0f;
	
	private int port = 27888;
	
	public GameObject firstPersonPrefab;
	public GameObject thirdPersonPrefab;
	
	// Serverside
	private NetworkPlayer[] player;
	private int currPlayer;
	//private int maxPlayer = 8;
	
	// Clientside 
	private string playerName = "Player";
	private string playerChatBox = "";
	private float chatboxSlider = 0.0f;
	private Object localPlayer; // Player View Model
	private NetworkConnectionError status;
	private string statusmsg;
	private List<ChatMessage> chatBuff = new List<ChatMessage>();
	
	// UI
	private string ipaddr = "127.0.0.1";
	
	void Start () {
		
	}

	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape)) {
			ToggleMenu();
		}
		
		if(Input.GetKeyUp(KeyCode.T))
			showMenu |= MENU_CHATBOX;
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		statusmsg = player.ipAddress+" connected";
		currPlayer++;
		showMenu |= MENU_CHATLATEST;
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.DestroyPlayerObjects(player);
		statusmsg = player.ipAddress+" disconnected";
		currPlayer--;
		showMenu = MENU_MAIN;
	}
	
	void HostServer() {
		statusmsg = "Hosting...";
		status = Network.InitializeServer(16, port, true);
		ToggleMenu();
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
		if(Network.isClient)
			Network.DestroyPlayerObjects(Network.player);
		
		Destroy(localPlayer);
		Network.Disconnect();
		showMenu = MENU_MAIN;
	}
	
	void OnConnectedToServer() {
		SpawnPlayer();
		ToggleMenu();
	}
	
	void OnDisconnectedFromServer() {
		
	}
	
	void SpawnPlayer() {
		//todo: spawnlocations
		localPlayer = Instantiate(firstPersonPrefab, transform.position, transform.rotation);
		Object thirdPerson = Network.Instantiate(thirdPersonPrefab, transform.position, transform.rotation, 0);
		Component movement = ((GameObject)thirdPerson).GetComponent("MovementSync");
		((MovementSync)movement).localPlayer = true;
		
		// hide local Third Person Mesh
		((GameObject)thirdPerson).GetComponentInChildren<SkinnedMeshRenderer>().renderer.enabled = false;
	}
	
	void Chat(string msg) {
		// make sure to be connected
		if(!Network.isClient&&!Network.isServer) return;
			networkView.RPC("BufferChatMessage", RPCMode.All, name, msg);
	}
	
	[RPC]
	void BufferChatMessage(string player, string msg) {
		ChatMessage chat;
		chat.message = msg;
		chat.player = playerName;
		chat.timestamp = Time.time;
		chatBuff.Add(chat);
		chatboxSlider = (float)chatBuff.Count;
	}

	void ToggleMenu() {
		if(showMenu!=MENU_INGAME) {
			// display default ingame UI
			lastMenu = showMenu;
			showMenu = MENU_INGAME;
			Screen.showCursor = false;
			Screen.lockCursor = true;
		} else {
			// get back to Main Menu
			showMenu=lastMenu;
			Screen.showCursor = true;
			Screen.lockCursor = false;
		}
	}
	
	void wndChatBox(int id) {
		GUI.SetNextControlName("chatbox"); 
		playerChatBox = GUI.TextField(new Rect(5,175,140,20), playerChatBox, 255);
		
		string text = "";
		
		if(chatBuff.Count>11){
			List<ChatMessage> selectedMessages = new List<ChatMessage>();
			
			chatboxSlider = GUI.VerticalScrollbar(new Rect(130,5,5,170), chatboxSlider, 11.0f, 0.0f, (float)(chatBuff.Count));			
			selectedMessages = chatBuff.GetRange((int)(chatboxSlider), 11);
			
			foreach(ChatMessage msg in selectedMessages) {
				text+=msg.player+": "+msg.message + "\n";
			}
			
			GUI.Label(new Rect(5,5,140,170), text);
			
		}
		else {
			foreach(ChatMessage msg in chatBuff) {
				text+=msg.player+": "+msg.message + "\n";
			}
			GUI.Label(new Rect(5,5,140,170), text);
		}
	}
	
	void OnGUI() {		
		if(showMenu==0) return;
		
		if((showMenu&MENU_CHATBOX)==MENU_CHATBOX) {
			if(Input.GetAxis("Mouse ScrollWheel")!=0.0f)
				chatboxSlider -= Input.GetAxis("Mouse ScrollWheel")*10.0f;
			
			// Escape chat box
			if(Event.current.keyCode==KeyCode.Escape||Input.GetButton("Fire1")||Input.GetButton("Fire2")||Input.GetButton("Fire3")) {
				showMenu &= ~MENU_CHATBOX;
				showMenu |= MENU_CHATLATEST;
			}
			// Send Chat
			else if((Event.current.keyCode==KeyCode.Return||Event.current.keyCode==KeyCode.KeypadEnter)) {
				showMenu &= ~MENU_CHATBOX;
				showMenu |= MENU_CHATLATEST;
				if(playerChatBox!="")
					Chat(playerChatBox);
				playerChatBox = "";
			} else {
				showMenu &= ~MENU_CHATLATEST;
				GUI.Window(0, new Rect(5,Screen.height-240,150,200), wndChatBox, "");
				GUI.FocusControl("chatbox");
			}
		}
		
		if((showMenu&MENU_CHATLATEST)==MENU_CHATLATEST) {
			List<ChatMessage> lastMessages = new List<ChatMessage>();
			string text = "";
			
			// search for the latest chat messages
			foreach(ChatMessage msg in chatBuff) {
				if(msg.timestamp<Time.time-CHAT_DISPLAYTIME) continue;
				lastMessages.Add(msg);
			}
			
			// limit to last 5 entries
			if(lastMessages.Count>5)
				lastMessages = lastMessages.GetRange(lastMessages.Count-5, 5);
			
			foreach(ChatMessage msg in lastMessages) {
				text += msg.player + ": " + msg.message + "\n";
			}
			
			GUI.Label(new Rect(5,Screen.height-180,300,100), text);
		}
		
		if((showMenu&MENU_MAIN)==MENU_MAIN) {
			if(!Network.isServer)
				if(GUI.Button(new Rect(0,0,100,30), "Host"))
					HostServer();
			
			if(Network.isClient||Network.isServer)
				if(GUI.Button(new Rect(0,130,100,30), "Disconnect"))
					Disconnect();
			
			if(Network.isServer) {				
				GUI.TextField(new Rect(0,Screen.height-0,100,30), (currPlayer+1)+" Player");
				GUI.TextField(new Rect(0,Screen.height-60,100,30), statusmsg + status.ToString());
			}
		}
		
		if((showMenu&MENU_CONNECT)==MENU_CONNECT) {
			ipaddr = GUI.TextArea(new Rect(0,100,100,30),ipaddr, 100);	
			if(GUI.Button(new Rect(0,130,100,30), "Connect")) {
				Connect(ipaddr);
			}
		}
	}
}