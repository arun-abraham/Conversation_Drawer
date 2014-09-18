using UnityEngine;
using System.Collections;

public class SimplePoint : MonoBehaviour {

	public AudioClip Gong;

	public bool pointMade;

	public GameObject lilPoint1;
	public GameObject lilPoint2;
	public GameObject lilPoint3;
	public GameObject lilPoint4;
	public GameObject lilPoint5;
	public GameObject lilPoint6;
	public GameObject lilPoint7;
	public GameObject lilPoint8;

	public float rotSpeed;
	private Vector3 rotVect;
	
	// Use this for initialization
	void Start () {

		rotSpeed = 4.0f;
		rotVect = new Vector3(0,0,1);
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate(rotVect * rotSpeed * Time.deltaTime);

		if(lilPoint1.GetComponent<Detail>().isHit && lilPoint2.GetComponent<Detail>().isHit && lilPoint3.GetComponent<Detail>().isHit && lilPoint4.GetComponent<Detail>().isHit &&
		   lilPoint5.GetComponent<Detail>().isHit && lilPoint6.GetComponent<Detail>().isHit && lilPoint7.GetComponent<Detail>().isHit && lilPoint8.GetComponent<Detail>().isHit) 
		{
			renderer.material.color = Color.cyan;
			//print("Good Point");
			pointMade = true;
			rotSpeed = 50.0f;
			audio.PlayOneShot(Gong);
			rotVect.y = 1;
			BroadcastMessage("IsHitOff");

		}
	
	}

}
