using UnityEngine;
using System.Collections;

public class MouseSeek : MonoBehaviour
{
	public Camera gameCamera = null;
	public SimpleMover mover;
	public Tracer tracer;

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
			if (tracer)
			{
				tracer.StartLine();
			}
		}
		else if (Input.GetMouseButton(0))
		{
			Drag();
		}
	}

	private void Drag(bool criticalLine = true)
	{
		Vector3 mousePosition = MousePointInWorld();
		Vector3 toMouse = mousePosition - transform.position;
		if (tracer == null || toMouse.sqrMagnitude > tracer.minDragToDraw * tracer.minDragToDraw)
		{
			float toMouseMag = toMouse.magnitude;
			if (toMouseMag > 0)
			{
				toMouse /= toMouseMag;
				float moveDist = mover.maxSpeed;
				mover.Move(toMouse, moveDist, true);
			}
			mover.moving = true;
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