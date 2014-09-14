using UnityEngine;
using System.Collections;

public class CastPoints : MonoBehaviour {


	public GameObject points;

	private Vector3 pointsPos;

	public bool isCreated = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		pointsPos = gameObject.transform.position;
	
	}

	void OnTriggerEnter (Collider collide)
	{
		if(collide.gameObject.tag == "Converser")
		{
			print("collided");
			//isCreated = true;
			if(gameObject.GetComponent<SimpleMover>().n)
				Instantiate(points, pointsPos, Quaternion.Euler(0,0,270));

			if(gameObject.GetComponent<SimpleMover>().s)
				Instantiate(points, pointsPos, Quaternion.Euler(0,0,90));

			if(gameObject.GetComponent<SimpleMover>().e)
				Instantiate(points, pointsPos, Quaternion.Euler(0,0,180));

			if(gameObject.GetComponent<SimpleMover>().w)
				Instantiate(points, pointsPos, Quaternion.Euler(0,0,0));

			if(gameObject.GetComponent<SimpleMover>().ne)
				Instantiate(points, pointsPos, Quaternion.Euler(0,0,225));

			if(gameObject.GetComponent<SimpleMover>().se)
				Instantiate(points, pointsPos, Quaternion.Euler(0,0,135));

			if(gameObject.GetComponent<SimpleMover>().sw)
				Instantiate(points, pointsPos, Quaternion.Euler(0,0,45));

			if(gameObject.GetComponent<SimpleMover>().nw)
				Instantiate(points, pointsPos, Quaternion.Euler(0,0,315));


		}
	}
}
