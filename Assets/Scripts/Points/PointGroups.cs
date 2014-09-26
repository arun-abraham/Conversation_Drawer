using UnityEngine;
using System.Collections;

public class PointGroups : MonoBehaviour {

	public bool fading = false;
	public bool bright = false;

	private float myAlpha;
	private float fadeConst = 0.2f;
<<<<<<< HEAD
=======

	public GameObject PointsGlobal;
>>>>>>> b18246a33356c4de08256a233f49aa5b0444e615


	// Use this for initialization
	void Start () {

		PointsGlobal = GameObject.FindGameObjectWithTag("Global Points");

		transform.parent = PointsGlobal.transform;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(fading == true)
		{
<<<<<<< HEAD
		BroadcastMessage("IsFading",SendMessageOptions.DontRequireReceiver);
=======
			BroadcastMessage("IsFading",SendMessageOptions.DontRequireReceiver);
>>>>>>> b18246a33356c4de08256a233f49aa5b0444e615
		}

		if(bright == true)
		{
<<<<<<< HEAD
		BroadcastMessage("IsBright",SendMessageOptions.DontRequireReceiver);
		}

		/*
		for(int i = transform.childCount-1;i>0;i--)
		{
			if (transform.GetChild(i).renderer != null)
			transform.GetChild(i).renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);
		}*/
=======
			BroadcastMessage("IsBright",SendMessageOptions.DontRequireReceiver);
		}

>>>>>>> b18246a33356c4de08256a233f49aa5b0444e615
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
