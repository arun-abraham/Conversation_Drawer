using UnityEngine;
using System.Collections;

public class CursorSeek : MonoBehaviour {
	public bool useController = false;
	public Camera gameCamera = null;
	public SimpleMover mover;
	public Tracer tracer;
	//public bool requireMouseDown;
	public bool directVelocity;
	private bool seeking;
	public GameObject cursor;
	public Tail tail;
	private Collider tailTrigger;
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

		if (tail != null)
		{
			tailTrigger = tail.trigger;
		}
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

		if (tracer != null)
		{
			if (tail != null)
			{
				tracer.AddVertex(tail.transform.position);
			}
			else
			{
				tracer.AddVertex(transform.position);
			}
		}
	}

	private void HandleTouches()
	{
		if (Input.GetMouseButtonDown(0))
		{
			seeking = !(toggleSeek && seeking);
		}
		else if ((!toggleSeek && Input.GetMouseButton(0)) || (toggleSeek && seeking))
		{
			Drag();
		}
		else
		{
			seeking = false;
			mover.SlowDown();
			if (tailTrigger != null)
			{
				tail.trigger.enabled = true;
			}
		}
	}


	private void FollowCursor()
	{
		if (cursor.GetComponent<ControllerSeek>().active)
		{
			if (!seeking)
			{
				seeking = true;
			}
			else
			{
				Drag();
			}
		}
		else
		{
			mover.SlowDown();
			seeking = false;
			if (tailTrigger != null)
			{
				tail.trigger.enabled = true;
			}
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
	}

	private Vector3 MousePointInWorld()
	{
		Vector3 touchPosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
		touchPosition.z = transform.position.z;
		return touchPosition;
	}

	private void TailStartFollow()
	{
		if (tracer != null)
		{
			tracer.StartLine();
		}
		if (tailTrigger != null)
		{
			tail.trigger.enabled = false;
		}
	}

	private void TailEndFollow()
	{
		if (tracer)
		{
			tracer.DestroyLine();
		}
	}
}
