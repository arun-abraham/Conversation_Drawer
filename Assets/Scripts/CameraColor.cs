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

		xColor = transform.position.x * 0.1f;
		yColor = transform.position.y * 0.1f;
		aColor = xColor + yColor;

		myColor = new Color(xColor,yColor,aColor,0);

		camera.backgroundColor = myColor;
	
	}
}
