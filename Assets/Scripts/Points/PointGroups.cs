using UnityEngine;
using System.Collections;

public class PointGroups : MonoBehaviour {

	public bool fading = false;
	public bool bright = false;

	private float myAlpha;
	private float fadeConst = 0.2f;


	// Use this for initialization
	void Start () {

		//gameObject.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(fading == true)
		{
		BroadcastMessage("IsFading",SendMessageOptions.DontRequireReceiver);
		}

		if(bright == true)
		{
		BroadcastMessage("IsBright",SendMessageOptions.DontRequireReceiver);
		}

		/*
		for(int i = transform.childCount-1;i>0;i--)
		{
			if (transform.GetChild(i).renderer != null)
			transform.GetChild(i).renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);
		}*/
	}

	public void IsFading()
	{
		fading = true;
		bright = false;
	}

	public void IsBright()
	{
		fading = false;
		bright = true;
	}
}
