using UnityEngine;
using System.Collections;

public class MiniPoint : MonoBehaviour {

	public bool isHit = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider collide)
	{
		if (collide.gameObject.tag == "Converser")
		{
			isHit = true;
			renderer.material.color = Color.cyan;
			audio.Play();
		}
		
	}
}
