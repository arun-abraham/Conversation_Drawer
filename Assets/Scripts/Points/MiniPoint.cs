using UnityEngine;
using System.Collections;

public class MiniPoint : MonoBehaviour {

	public bool isHit = false;
	public bool isplayed = false;

	private float myAlpha;
	private float fadeConst = 0.2f;
	public bool fading = false;
	public bool bright = false;
	public GameObject creator;
	public float informationFactor;

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
		/*

	if(Input.GetKeyDown(KeyCode.Space))
		{
			print("Space up");
			fading = true;
			bright = false;
		}

	if(Input.GetKeyUp(KeyCode.Space))
		{
			print ("Space down");
			bright = true;
			fading = false;

		}*/
	
	}

	void OnTriggerEnter (Collider collide)
	{
		if (collide.gameObject.tag == "Converser" && collide.gameObject != creator)
		{
			isHit = true;
			renderer.material.color = Color.cyan;

			if(isplayed == false)
			audio.Play();

			isplayed = true;

			collide.gameObject.BroadcastMessage("UnderstandPoint", informationFactor);
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
