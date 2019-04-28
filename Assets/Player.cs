using UnityEngine;
using System.Collections;
using Photon;


public class Player : PunBehaviour
{
	public float speed = 3f;
	public static Player instance;

	void Awake ()
	{
		instance = this;
	}

	void Update ()
	{
		if (photonView.isMine)
		{
			InputColorChange ();
			InputMovement ();
		}
	}

	void InputMovement ()
	{
		if (Input.GetKey (KeyCode.W))
		{
			Vector3 pos = new Vector3 (0, 0, speed * Time.deltaTime);
			transform.Translate (pos);
		}
		if (Input.GetKey (KeyCode.S))
		{
			Vector3 pos = new Vector3 (0, 0, -(speed * Time.deltaTime));
			transform.Translate (pos);
		}
		if (Input.GetKey (KeyCode.D))
		{
			Vector3 pos = new Vector3 (speed * Time.deltaTime, 0, 0);
			transform.Translate (pos);
		}
		if (Input.GetKey (KeyCode.A))
		{
			Vector3 pos = new Vector3 (-(speed * Time.deltaTime), 0, 0);
			transform.Translate (pos);
		}
	}

	public void InputColorChange ()
	{
		if (Input.GetKeyDown (KeyCode.R))
			ChangeColorTo (new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f)));
	}

	[PunRPC] 
	void ChangeColorTo (Vector3 color)
	{
		transform.GetComponent<Renderer> ().material.color = new Color (color.x, color.y, color.z, 1f);
		if (photonView.isMine)
			photonView.RPC ("ChangeColorTo", PhotonTargets.Others, color);
	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
			stream.SendNext (transform.position);
		else
			transform.position = (Vector3)stream.ReceiveNext ();
	}
}