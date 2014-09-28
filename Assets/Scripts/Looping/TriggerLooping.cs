using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerLooping : MonoBehaviour {
	
	public GameObject player;

	private float moveDistanceVertical = 0;
	private float moveDistanceHorizontal = 0;

	public enum ColliderLocation{Top,Bottom,Left,Right};

	public float worldWidth = 200f;
	public float worldHeight = 200f;

	void Start()
	{
		ChangeWorldSize(200f, 200f);
	}

	void Update()
	{
		ChangeWorldSize(worldWidth, worldHeight);
	}

	void ChangeWorldSize(float worldWidth, float worldHeight)
	{
		foreach(Transform child in transform)
		{
			
			switch(child.GetComponent<Boundary>().colliderLocation)
			{
				case ColliderLocation.Top:
					//Resize Collider
					child.GetComponent<BoxCollider>().size = new Vector3(worldWidth, 1.0f, 1.0f);
					child.transform.localPosition = new Vector3(0f, worldHeight/2, 0f);
					break;
				case ColliderLocation.Bottom:
					//Resize Collider
					child.GetComponent<BoxCollider>().size = new Vector3(worldWidth, 1.0f, 1.0f);
					child.transform.localPosition = new Vector3(0f, -worldHeight/2, 0f);
					break;
				case ColliderLocation.Left:
					//Resize Collider
					child.GetComponent<BoxCollider>().size = new Vector3(1.0f, worldHeight, 1.0f);
					child.transform.localPosition = new Vector3(-worldWidth/2, 0f, 0f);
					break;
				case ColliderLocation.Right:
					//Resize Collider
					child.GetComponent<BoxCollider>().size = new Vector3(1.0f, worldHeight, 1.0f);
					child.transform.localPosition = new Vector3(worldWidth/2, 0f, 0f);
					break;
			}
		}

		moveDistanceHorizontal = worldWidth - 10f;
		moveDistanceVertical = worldHeight - 10f;
	}


	public void MoveWorld(ColliderLocation location)
	{
		Vector3 moveDistance = Vector3.zero;

		switch(location)
		{
			case ColliderLocation.Top:
				moveDistance = new Vector3(0f, moveDistanceVertical, 0f);
				break;
			case ColliderLocation.Bottom:
			moveDistance = new Vector3(0f, -moveDistanceVertical, 0f);
				break;
			case ColliderLocation.Left:
			moveDistance = new Vector3(-moveDistanceHorizontal, 0f, 0f);
				break;
			case ColliderLocation.Right:
			moveDistance = new Vector3(moveDistanceHorizontal,0f,0f);
				break;
		}

		//Move the boundaries
		transform.position += moveDistance;

			//Get all gameobjects in the scene and then filter to get only loopable objects//
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
			List<GameObject> loopableObjects = new List<GameObject>();
			foreach(GameObject go in allObjects)
			{
				if(go.GetComponent<LoopTag>() != null)				
						loopableObjects.Add(go);
			}

			foreach(GameObject lo in loopableObjects)
			{
				
				//Check if the object is on screen
				if (!OnScreen(lo))
				{
					if(lo.GetComponent<LoopTag>().moveRoot)
						lo.transform.root.position += moveDistance;
					else
						lo.transform.position += moveDistance;

					if(OutSideBounds(lo,location))
						lo.transform.position -= moveDistance;

				Debug.Log(OnScreen(lo));
					MoveOffScreen(lo, location);	
				}
			}

		
	}

	public void MoveIndividual(ColliderLocation location, GameObject individual)
	{

			if (!OnScreen(individual))
			{
				Vector3 moveDistance = Vector3.zero;
				switch(location)
				{
				case ColliderLocation.Top:
					moveDistance = new Vector3(0f, moveDistanceVertical, 0f);
					break;
				case ColliderLocation.Bottom:
					moveDistance = new Vector3(0f, -moveDistanceVertical, 0f);
					break;
				case ColliderLocation.Left:
					moveDistance = new Vector3(-moveDistanceHorizontal, 0f, 0f);
					break;
				case ColliderLocation.Right:
					moveDistance = new Vector3(moveDistanceHorizontal,0f,0f);
					break;
				}

				Tracer otherTracer = individual.GetComponent<Tracer>();
				Vector3 oldPosition = individual.transform.position;

			if (individual.gameObject.GetComponent<LoopTag>().moveRoot)
				{
					individual.gameObject.transform.root.position -= moveDistance;
				}
				else
				{
					individual.gameObject.transform.position -= moveDistance;
				}
				
				MoveOffScreen(individual.gameObject,location);
				otherTracer.MoveVertices(individual.transform.position - oldPosition);
			}

	}

	private bool OutSideBounds(GameObject lo, ColliderLocation colliderLocation)
	{
		switch (colliderLocation)
		{
		case ColliderLocation.Top:
			return lo.transform.position.y > transform.position.y;
		case ColliderLocation.Bottom:
			return lo.transform.position.y < transform.position.y;
		case ColliderLocation.Left:
			return lo.transform.position.x < transform.position.x;
		case ColliderLocation.Right:
			return lo.transform.position.x > transform.position.x;
		}

		return false;
	}

	private bool OnScreen(GameObject lo)
	{
		Vector3 newPos = Camera.main.WorldToViewportPoint(lo.transform.position);

		bool onScreen = false;
		if(newPos.x > -0.01f && newPos.x < 1.01f && newPos.y > -0.01f && newPos.y < 1.01f)
			onScreen = true;
		return onScreen;

	}

	private void MoveOffScreen(GameObject lo, ColliderLocation colliderLocation)
	{
		Debug.Log(moveDistanceVertical);

		if (OnScreen(lo))
		{
			var camHeight = Camera.main.orthographicSize;
			var camWidth = camHeight * Camera.main.aspect;
			//Switch to see which direction to move by viewport size
			switch (colliderLocation)
			{
			case ColliderLocation.Top:
				Debug.Log (camHeight);
				Debug.Log(lo.transform.position);
				lo.transform.root.position += new Vector3(0, camHeight, 0);
				Debug.Log(lo.transform.position);
				break;
			case ColliderLocation.Bottom:
				lo.transform.position -= new Vector3(0,camHeight, 0);
				break;
			case ColliderLocation.Left:
				lo.transform.position -= new Vector3(camWidth, 0, 0);
				break;
			case ColliderLocation.Right:
				lo.transform.position += new Vector3(camWidth, 0, 0);
				break;
			}
		}

		if(OnScreen(lo))
		{
			MoveOffScreen(lo, colliderLocation);
		}
	}


}
