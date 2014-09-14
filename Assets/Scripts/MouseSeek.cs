using UnityEngine;
using System.Collections;

public class MouseSeek : MonoBehaviour
{
	public Camera gameCamera = null;
	public SimpleMover mover;
	public Tracer tracer;
	public bool toggleSeek;
	public bool directVelocity;
	private bool startedLine;

	void Start()
	{
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
	}

	void Update()
	{
		HandleTouches();
	}

	private void HandleTouches()
	{
		if (Input.GetMouseButtonDown(0))
		{
			startedLine = !(toggleSeek && startedLine);
			if (tracer)
			{
				tracer.StartLine();
			}
		}
		else if ((!toggleSeek && Input.GetMouseButton(0)) || (toggleSeek && startedLine))
		{
			Drag();
		}
		else
		{
			startedLine = false;
			mover.SlowDown();
		}
	}

	private void Drag(bool criticalLine = true)
	{
		Vector3 mousePosition = MousePointInWorld();
		Vector3 toMouse = mousePosition - transform.position;
		if (tracer == null || toMouse.sqrMagnitude > Mathf.Pow(tracer.minDragToDraw, 2))
		{
			if (directVelocity)
			{
				mover.Move(toMouse.normalized, mover.maxSpeed, true);
			}
			else
			{
				mover.Accelerate(toMouse);
			}
			if (tracer != null)
			{
				tracer.AddVertex(transform.position);
			}
		}
	}

	private Vector3 MousePointInWorld()
	{
		Vector3 touchPosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
		touchPosition.z = transform.position.z;
		return touchPosition;
	}
}