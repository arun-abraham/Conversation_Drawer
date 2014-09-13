using UnityEngine;
using System.Collections;

public class CursorSeek : MonoBehaviour {

	public Camera gameCamera = null;
	public SimpleMover mover;
	public Tracer tracer;
	public float acceleration;
	public bool requireMouseDown;
	public bool directVelocity;
	private bool startedLine;
	public GameObject cursor;

	// Use this for initialization
	void Start () {
		if(gameCamera == null)
		{
			gameCamera = Camera.main;
		}
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		tracer.StartLine();
	}
	
	// Update is called once per frame
	void Update () {
		FollowCursor();
	}

	private void FollowCursor()
	{
		//if (cursor.active)
	
		if (cursor.GetComponent<ControllerSeek>().active)
		{
			if (tracer)
			{
				startedLine = true;
				Drag();
			}
		}
		else
		{
			mover.SlowDown();
		}
	}

	private void Drag(bool criticalLine = true)
	{
		//Vector3 mousePosition = MousePointInWorld();
		//Vector3 toMouse = mousePosition - transform.position;

		//if (tracer == null || cursor.GetComponent<ControllerSeek>.forward  Mathf.Pow(tracer.minDragToDraw, 2))
		{
			if (directVelocity)
			{
				mover.Move(cursor.GetComponent<ControllerSeek>().forward.normalized, mover.maxSpeed, true);
			}
			else
			{
				mover.Accelerate(cursor.GetComponent<ControllerSeek>().forward.normalized * acceleration);
			}
			if (tracer != null)
			{
				tracer.AddVertex(transform.position);
			}
		}
	}
}
