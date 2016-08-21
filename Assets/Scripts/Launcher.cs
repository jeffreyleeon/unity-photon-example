using UnityEngine;

namespace Com.FYP.DES {

	public class Launcher : Photon.PunBehaviour {
		// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking change)
		string _gameVersion = "1";
		bool isConnecting;

		[Tooltip("The Ui Panel to let the user enter name, connect and play")]
		public GameObject controlPanel;
		[Tooltip("The UI Label to inform the user that the connection is in progress")]
		public GameObject progressLabel;

		void Start () {
			SetLoading (false);
		}

		void Awake () {
			PhotonNetwork.logLevel = PhotonLogLevel.Full;
			PhotonNetwork.autoJoinLobby = false;
			PhotonNetwork.automaticallySyncScene = true;
		}

		private void SetLoading (bool loading) {
			progressLabel.SetActive(loading);
			controlPanel.SetActive(!loading);
		}

		public void Connect () {
			SetLoading (true);
			isConnecting = true;
			if (PhotonNetwork.connected) {
				print ("====jeffrey JoinRandomRoom");
				// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed()
				PhotonNetwork.JoinRandomRoom ();
			} else {
				print ("====jeffrey ConnectUsingSettings");
				PhotonNetwork.ConnectUsingSettings (_gameVersion);
			}
		}

		public override void OnConnectedToMaster () {
			print ("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
			// #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()  
			if (isConnecting) {
				PhotonNetwork.JoinRandomRoom();
			}
		}

		public override void OnDisconnectedFromPhoton () {
			print ("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
			SetLoading (false);
		}

		public override void OnPhotonRandomJoinFailed (object[] codeAndMsg) {
			print ("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 2}, null);");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom (
				"Jeffrey roomName",
				new RoomOptions () {
					maxPlayers = 2
				},
				null
			);
		}

		public override void OnJoinedRoom () {
			print ("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
			PrintRooms ();
			progressLabel.SetActive(false);
			controlPanel.SetActive(false);
			if (PhotonNetwork.room.playerCount == 1) {
				Debug.Log("We load the 'Room for 1' ");
				// #Critical
				// Load the Room Level. 
				PhotonNetwork.LoadLevel("Room for 1");
			}
		}

		private void PrintRooms () {
			RoomInfo[] rooms = PhotonNetwork.GetRoomList ();
			print ("Room count: " + rooms.Length);
			for (int i = 0; i < rooms.Length; ++i) {
				RoomInfo room = rooms [i];
				print ("Room info: " + room.GetHashCode () + ":" + room.name);
			}
		}
	}

}
