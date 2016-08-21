using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

namespace Com.FYP.DES {
	public class GameManager : Photon.PunBehaviour {

		public override void OnLeftRoom () {
			print ("===========OnLeftRoom callback");
			SceneManager.LoadScene (0);
		}

		public void LeaveRoom () {
			print ("===========GameManager LeaveRoom");
			PhotonNetwork.LeaveRoom ();
		}

		void LoadArena () {
			if (!PhotonNetwork.isMasterClient) {
				print ("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
			print ("PhotonNetwork : Loading Level : " + PhotonNetwork.room.playerCount);
			PhotonNetwork.LoadLevel ("Room for " + PhotonNetwork.room.playerCount);
		}

		public override void OnPhotonPlayerConnected( PhotonPlayer other  )
		{
			Debug.Log( "OnPhotonPlayerConnected() " + other.name ); // not seen if you're the player connecting
			if ( PhotonNetwork.isMasterClient ) {
				Debug.Log( "OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient ); // called before OnPhotonPlayerDisconnected
				LoadArena();
			}
		}


		public override void OnPhotonPlayerDisconnected( PhotonPlayer other  )
		{
			Debug.Log( "OnPhotonPlayerDisconnected() " + other.name ); // seen when other disconnects
			if ( PhotonNetwork.isMasterClient ) {
				Debug.Log( "OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient ); // called before OnPhotonPlayerDisconnected
				LoadArena();
			}
		}
	}
}