using UnityEngine;
using System.Collections;

public class CameraColor : MonoBehaviour {

	private Color myColor;
	private float xColor;
	private float yColor;
	private float aColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(this.GetComponent<PartnerLink>().partner == false)
		{
			camera.backgroundColor = Color.black;
		}

		if(this.GetComponent<PartnerLink>().partner == true)
		{

		xColor = 0 + transform.position.x * 0.1f;
		yColor = 0 + transform.position.y * 0.1f;
		aColor = 0 + xColor + yColor;

		myColor = new Color(xColor,yColor,aColor,0);

		camera.backgroundColor = myColor;
		}
	
	}
}
