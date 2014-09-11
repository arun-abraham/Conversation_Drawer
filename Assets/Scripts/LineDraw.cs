using UnityEngine;
using System.Collections;

public class LineDraw : MonoBehaviour {

	private LineRenderer lineRenderer;

	public GameObject pointMade1;
	public GameObject pointMade2;
	


	// Use this for initialization
	void Start () {

		lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.SetColors(Color.blue, Color.cyan);
	
		
	}
	
	// Update is called once per frame
	void Update () {

		if(pointMade1.GetComponent<SimplePoint>().pointMade && pointMade2.GetComponent<SimplePoint>().pointMade)
		{
			lineRenderer.SetPosition(0, pointMade1.transform.position);
			lineRenderer.SetPosition(1, pointMade2.transform.position);

		}


	}

}
