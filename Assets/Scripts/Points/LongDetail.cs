using UnityEngine;
using System.Collections;

public class LongDetail : MonoBehaviour {

	public Material myMaterial;
	
	public bool isHit = false;

	public bool allDone = false;

	private float myAlpha;
	private float fadeConst = 0.2f;
	public bool fading = false;
	public bool bright = false;
	
	// Use this for initialization
	void Start () {

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

			if (allDone == false)
			audio.Play();

			Invoke("setHitOff",5.0f);
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