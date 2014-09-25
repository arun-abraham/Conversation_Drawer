using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerLooping : MonoBehaviour {

	public List<GameObject> listOfObjectsToLoop = new List<GameObject>();
	public List<GameObject> listOfCollidersToMove = new List<GameObject>();
	public GameObject player;

	public enum ColliderLocation{Top,Bottom,Left,Right};
	public ColliderLocation colliderLocation;

	private float orthoSize = 0;
	private float aspect = 0;

	/*Debug Variables
	public float xCollider = 1;
	public float yCollider = 1;
	private float cameraHeight = 0;
	private float cameraWidth = 0;*/

	private Vector3 loopMoveDistance = Vector3.zero;

	void Start()
	{

		switch(colliderLocation)
		{
		case ColliderLocation.Top:
			loopMoveDistance.y = 398f;
			break;
		case ColliderLocation.Bottom:
			loopMoveDistance.y = -398f;
			break;
		case ColliderLocation.Left:
			loopMoveDistance.x = -398f;
			break;
		case ColliderLocation.Right:
			loopMoveDistance.x = 398f;
			break;
		}

	}

	void Update()
	{
		//boundaryCollider.size = new Vector3(xCollider, yCollider, 10);


		//World Measurements
		//cameraHeight = Camera.main.orthographicSize * 2f;
		//cameraWidth = cameraHeight * Camera.main.aspect;

		/* Debug Tests
		Debug.Log ("Orthographic Size: "+Camera.main.orthographicSize);
		Debug.Log ("Aspect Ratio: "+Camera.main.aspect);
		Debug.Log ("Camera Height: " + cameraHeight);
		Debug.Log ("Camera Width: " + cameraWidth);
		*/


	}

	//TODO: CLEAN THIS UP AND MAKE IT WORK FOR RIGHT AND LEFT

	void OnTriggerEnter(Collider other)
	{

		//Debug.Log("Trigger");
		if(other.gameObject == player)
		{
			//Get all gameobjects in the scene and then filter to get only loopable objects//
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
			foreach(GameObject go in allObjects)
			{
				if(go.GetComponent<LoopTag>() != null)
				{
					if(go.GetComponent<LoopTag>().boundaryCollider)
						listOfCollidersToMove.Add(go);
					else
						listOfObjectsToLoop.Add(go);
				}
			}

			foreach(GameObject lo in listOfCollidersToMove)
			{
				lo.transform.position += loopMoveDistance;			
			}

			//////////////////Handle moving the loopable objects//////////////////////////////
			foreach(GameObject lo in listOfObjectsToLoop)
			{
					
				//Check if the object is on screen
				if (OnScreen(lo))
				{
					if(lo.GetComponent<LoopTag>().loopOnCamera)
					{
						if(lo.GetComponent<LoopTag>().moveRoot)
							lo.transform.root.position += loopMoveDistance;
						else
							lo.transform.position += loopMoveDistance;

					}
				}
				else
				{
				
						if(lo.GetComponent<LoopTag>().moveRoot)
							lo.transform.root.position += loopMoveDistance;
						else
							lo.transform.position += loopMoveDistance;

					if(OutSideBounds(lo))
						lo.transform.position -= loopMoveDistance;

							MoveOffScreen(lo);
												

				}
			}

			listOfObjectsToLoop.Clear();
			listOfCollidersToMove.Clear();
		}

		//Debug.Log(other.transform.name);
		if(other.tag == "Converser" && other.transform != player.transform)
		{
			//Debug.Log("HERE");
			if (!OnScreen(other.gameObject))
			{
				
				if(other.gameObject.GetComponent<LoopTag>().moveRoot)
					other.gameObject.transform.root.position -= loopMoveDistance;
				else
					other.gameObject.transform.position -= loopMoveDistance;

				MoveOffScreen(other.gameObject);
			}
		}
	}

	private bool OutSideBounds(GameObject lo)
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

	private void MoveOffScreen(GameObject lo)
	{

		if (OnScreen(lo))
		{
			var camHeight = Camera.main.orthographicSize;
			var camWidth = camHeight * Camera.main.aspect;
			//Switch to see which direction to move by viewport size
			switch (colliderLocation)
			{
			case ColliderLocation.Top:
				lo.transform.position += new Vector3(0, camHeight, 0);
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
	}


}
