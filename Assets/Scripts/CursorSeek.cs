using UnityEngine;
using System.Collections;

public class CursorSeek : MonoBehaviour {
	public bool useController = false;
	public Camera gameCamera = null;
	public SimpleMover mover;
	public Tracer tracer;
	//public bool requireMouseDown;
	public bool directVelocity;
	private bool startedLine;
	public GameObject cursor;
	public bool toggleSeek;

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
		if (useController)
		{ 
			FollowCursor(); 
		}
		else
		{
			HandleTouches();
		}
	}

	private void HandleTouches()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!startedLine)
			{
				if (tracer)
				{
					tracer.StartLine();
				}
			}
			startedLine = !(toggleSeek && startedLine);
		}
		else if ((!toggleSeek && Input.GetMouseButton(0)) || (toggleSeek && startedLine))
		{
			Drag();
		}
		else
		{
			startedLine = false;
			mover.SlowDown();
			if (tracer)
			{
				tracer.DestroyLine();
			}
		}
	}


	private void FollowCursor()
	{
		if (cursor.GetComponent<ControllerSeek>().active)
		{
			if (!startedLine)
			{
				startedLine = true;
				if (tracer != null)
				{
					tracer.StartLine();
				}
			}
			else
			{
				Drag();
			}
				
		}
		else
		{
			mover.SlowDown();
			if (tracer)
			{
				tracer.DestroyLine();
			}
			startedLine = false;
		}
	}

	private void Drag(bool criticalLine = true)
	{
		Vector3 dragForward = MousePointInWorld() - transform.position;
		if (useController)
		{
			dragForward = cursor.GetComponent<ControllerSeek>().forward;
		}

		if (directVelocity)
		{
			mover.Move(dragForward, mover.maxSpeed, true);
		}
		else
		{
			mover.Accelerate(dragForward);
		}
		if (tracer != null)
		{
			tracer.AddVertex(transform.position);
		}
	}

	private Vector3 MousePointInWorld()
	{
		Vector3 touchPosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
		touchPosition.z = transform.position.z;
		return touchPosition;
	}
}
