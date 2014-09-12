using UnityEngine;
using System.Collections;

public class LongDetail : MonoBehaviour {

	public Material myMaterial;
	
	public bool isHit = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(isHit == true)
		{
			Invoke("setHitOff",3.0f);
		}
		
	}
	
	void OnTriggerEnter (Collider collide)
	{
		if (collide.gameObject.tag == "Player")
		{
			setHitOn();
			audio.Play();
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
		renderer.material.color = Color.blue;
	}

	
	
}