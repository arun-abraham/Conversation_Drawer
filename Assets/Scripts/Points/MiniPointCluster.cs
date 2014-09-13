using UnityEngine;
using System.Collections;

public class MiniPointCluster : MonoBehaviour {

	public AudioClip Gong;
	public float rotSpeed;
	private Vector3 rotDir;

	public GameObject point1;
	public GameObject point2;
	public GameObject point3;
	public GameObject point4;

	// Use this for initialization
	void Start () {
		
		rotSpeed = 6.0f;
		rotDir = new Vector3(0,0,-1);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Rotate(rotDir * rotSpeed * Time.deltaTime);

		if(point1.GetComponent<MiniPoint>().isHit && point2.GetComponent<MiniPoint>().isHit && point3.GetComponent<MiniPoint>().isHit && point4.GetComponent<MiniPoint>().isHit)
		{
			//audio.PlayOneShot(Gong);
			rotSpeed = 50.0f;
			rotDir.y = 1;
			//BroadcastMessage("IsHitOff");
		}
		
	}
}
