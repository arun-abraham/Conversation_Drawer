using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerLooping : MonoBehaviour {

	public List<GameObject> listOfObjectsToLoop = new List<GameObject>();


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
			loopMoveDistance.y = 400f;
			break;
		case ColliderLocation.Bottom:
			loopMoveDistance.y = -400f;
			break;
		case ColliderLocation.Left:
			break;
		case ColliderLocation.Right:
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

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Converser")
		{
			//Get all gameobjects in the scene and then filter to get only loopable objects//
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
			foreach(GameObject go in allObjects)
			{
				if(go.GetComponent<LoopTag>() != null)
				{
					listOfObjectsToLoop.Add(go);
				}
			}

			//////////////////Handle moving the loopable objects//////////////////////////////
			foreach(GameObject lo in listOfObjectsToLoop)
			{
				//If we are moving the world boundaries, skip everything else
				if(lo.GetComponent<LoopTag>().boundaryCollider)
				{
					lo.transform.position += loopMoveDistance;			
					continue;
				}
					
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

					MoveOffScreen(lo);							

				}
			}

			listOfObjectsToLoop.Clear();
		}
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
			var camHeight = Camera.main.orthographicSize * 2f;
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
