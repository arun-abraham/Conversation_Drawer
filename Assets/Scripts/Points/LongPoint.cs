using UnityEngine;
using System.Collections;

public class LongPoint : MonoBehaviour {

	public CoolEvent coolEvent;
	public bool pointMade;

	public AudioClip Gong;
	
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
	
	private float myAlpha;
	private float fadeConst = 0.2f;
	public bool fading = false;
	public bool bright = false;

	// Use this for initialization
	void Start () {
		
		rotSpeed = 5.0f;
		rotVect = new Vector3(0,0,1);
		myAlpha = 1;
		
	}
	
	// Update is called once per frame
	void Update () {

		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);

		if(fading == true)
		{
			if(myAlpha >= 0)
				myAlpha -= Time.deltaTime * fadeConst;
		}
		
		if(bright == true)
		{
			if(myAlpha <=1)
				myAlpha += Time.deltaTime * fadeConst;
		}

		transform.Rotate(rotVect * rotSpeed * Time.deltaTime);
		
		if(lilPoint1.GetComponent<LongDetail>().isHit && lilPoint2.GetComponent<LongDetail>().isHit && lilPoint3.GetComponent<LongDetail>().isHit && lilPoint4.GetComponent<LongDetail>().isHit &&
		   lilPoint5.GetComponent<LongDetail>().isHit && lilPoint6.GetComponent<LongDetail>().isHit && lilPoint7.GetComponent<LongDetail>().isHit && lilPoint8.GetComponent<LongDetail>().isHit) 
		{
			renderer.material.color = Color.cyan;
			//print("Good Point");
			pointMade = true;
			rotSpeed = 50.0f;
			audio.PlayOneShot(Gong);
			rotVect.y = 2;
			BroadcastMessage("IsHitOff");

			if (coolEvent != null)
			{
				coolEvent.BeCool();
			}
		}
		
	}

	public void IsFading()
	{
		fading = true;
		bright = false;
		//print ("Is fading");
	}
	
	public void IsBright()
	{
		fading = false;
		bright = true;
		//print ("Is Bright");
	}



	
}