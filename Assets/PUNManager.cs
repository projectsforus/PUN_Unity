using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.UI;

public class PUNManager : PunBehaviour
{
	public Text title;
	public GameObject ChangeColorBtn;
	public GameObject joinRoomBtn;
	public Text status;
	public GameObject Plane;
	public GameObject playerPrefab;
	RoomOptions roomOption;
	public PhotonView view;

	void Awake ()
	{
		PhotonNetwork.ConnectUsingSettings ("0.1");
		roomOption = new RoomOptions ()
		{ IsOpen = true, MaxPlayers = 2, IsVisible = true };
	}
	// Use this for initialization
	void Start ()
	{
		//view = GetComponent<PhotonView> ();
		joinRoomBtn.SetActive (false);
		Plane.SetActive (false);
		ChangeColorBtn.SetActive (false);
		title.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update ()
	{
		if (!PhotonNetwork.connected)
		{
			status.text = PhotonNetwork.connectionStateDetailed.ToString ();// "Connecting..";
		}
	}

	public override void OnJoinedLobby ()
	{
		joinRoomBtn.SetActive (true);
		status.text = "Connected";
		Debug.Log ("Joined lobby: " + PhotonNetwork.lobby.Name);
	}

	public void JoinRoom ()
	{
		status.text = "Connecting to room..";
		PhotonNetwork.JoinRandomRoom ();
	}
	// This callback is called when random room join fails. Thus creating a new room
	public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
	{
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom (null, roomOption, null);
	}

	public override void OnJoinedRoom ()
	{
		Debug.Log ("Connected to Room");
		status.text = "";
		joinRoomBtn.SetActive (false);
		Plane.SetActive (true);
		ChangeColorBtn.SetActive (true);
		title.gameObject.SetActive (true);
		PhotonNetwork.Instantiate (playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
	}

	// This callback is called when other player is connected to the room
	public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer)
	{
		Debug.Log ("Other player arrived : " + newPlayer + " && id : " + newPlayer.ID);
	}

	public void ChangeTextColor ()
	{
		Vector3 color = new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));

		view.RPC ("ChangeTextColorTo", PhotonTargets.All, color);
	}

	[PunRPC] 
	void ChangeTextColorTo (Vector3 color)
	{
		title.color = new Color (color.x, color.y, color.z, 1f);
		Debug.Log ("Color Called");
	}
}
