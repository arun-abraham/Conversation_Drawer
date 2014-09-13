using UnityEngine;
using System.Collections;

public class LongDetail : MonoBehaviour {

	public Material myMaterial;
	
	public bool isHit = false;

	public bool allDone = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if(allDone == true)
		{
			renderer.material.color = Color.blue;
		}

	}
	
	void OnTriggerEnter (Collider collide)
	{
		if (collide.gameObject.tag == "Converser")
		{
			setHitOn();
			audio.Play();
			Invoke("setHitOff",3.0f);
		}
		
	}
	
	void setHitOn ()
	{
		isHit = true;
		renderer.material.color = Color.blue;
	}
	
	void setHitOff ()
	{
		isHit = false;
		renderer.material = myMaterial;
	}

	void IsHitOff ()
	{
		isHit = false;
		allDone = true;
	}

	
	
}