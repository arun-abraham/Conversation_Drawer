using UnityEngine;
using System.Collections;

public class DeepDetail : MonoBehaviour {

public Material myMaterial;
	
public bool isHit = false;
private bool isHitOnce = false;
private bool waiting = false;
	
// Use this for initialization
void Start () {
		
	}
	
// Update is called once per frame
void Update () {

	}
	
	void OnTriggerEnter (Collider collide)
	{
		if (collide.gameObject.tag == "Player")
		{
			if(isHitOnce == false && isHit == false)
			{
				setHitOnce();
				waiting = true;
				Invoke("setWaiting", 1.0f);
			}

			if(isHitOnce == true && waiting == false)
				setHitOn();

			if(isHit == true)
				setHitOn();

			audio.Play();
		}
		
	}
	
	
	void setHitOnce ()
	{
		isHitOnce = true;
		renderer.material.color = Color.red; 
		Invoke("setHitOnceOff",5.0f);
	}
	
	void setHitOnceOff ()
	{
		isHitOnce = false;
		renderer.material = myMaterial;  
	}
	
	void setHitOn ()
	{
		isHit = true;
		renderer.material.color = Color.blue;
		Invoke("setHitOff",5.0f);
	}
	
	void setHitOff ()
	{
		isHit = false;
		setHitOnce();
		renderer.material.color = Color.red;
	}

	void setWaiting ()
	{
		waiting = false;
	}

	void IsHitOff ()
	{
		isHit = false;
		renderer.material.color = Color.blue;
	}

	
	
}