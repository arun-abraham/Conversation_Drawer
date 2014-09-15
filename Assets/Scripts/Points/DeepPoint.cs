using UnityEngine;
using System.Collections;

public class DeepPoint : MonoBehaviour {

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
	
	rotSpeed = 6.0f;
	rotVect = new Vector3(0,0,-1);
	
}

// Update is called once per frame
void Update () {
	
	transform.Rotate(rotVect * rotSpeed * Time.deltaTime);
	
	if(lilPoint1.GetComponent<DeepDetail>().isHit && lilPoint2.GetComponent<DeepDetail>().isHit && lilPoint3.GetComponent<DeepDetail>().isHit && lilPoint4.GetComponent<DeepDetail>().isHit &&
	   lilPoint5.GetComponent<DeepDetail>().isHit && lilPoint6.GetComponent<DeepDetail>().isHit && lilPoint7.GetComponent<DeepDetail>().isHit && lilPoint8.GetComponent<DeepDetail>().isHit) 
	{
		renderer.material.color = Color.cyan;
		print("Good Point");
		pointMade = true;
		rotSpeed = 50.0f;
		audio.PlayOneShot(Gong);
		rotVect.y = 3;
		BroadcastMessage("IsHitOff");
	}
	
}

}
