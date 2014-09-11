using UnityEngine;
using System.Collections;

public class ScrollDrag : MonoBehaviour {
	public Vector3 primaryDirection = new Vector3(0, -1, 0);
	public float minSpeed;
	public float maxSpeed;
	public float currentSpeed;

	void Start()
	{
		if (minSpeed > maxSpeed)
		{
			float tempSpeed = minSpeed;
			minSpeed = maxSpeed;
			maxSpeed = tempSpeed;
		}
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			CalculateSpeed();
		}
		else
		{
			currentSpeed = minSpeed;
		}

		Vector3 movement = primaryDirection * currentSpeed;
		transform.position += movement * Time.deltaTime;
	}

	private void CalculateSpeed()
	{
		// Separate mouse position in terms of the primary direction.
		Vector3 mouseInverseY = Input.mousePosition;
		mouseInverseY.y = Screen.height - mouseInverseY.y;
		Vector3 mouseParallel = ProjectVector(primaryDirection, mouseInverseY);
		Vector3 mousePerpendicular = ProjectVector(Vector3.Cross(primaryDirection, Vector3.forward), mouseInverseY);

		// Calculate speed based on how far along the primary direction the mouse is.
		Vector3 screenParallel = ProjectVector(primaryDirection, new Vector3(Screen.width, Screen.height));
		float speedFactor = mouseParallel.magnitude / screenParallel.magnitude;
		currentSpeed = (speedFactor * (maxSpeed - minSpeed)) + minSpeed;
	}

	private Vector3 ProjectVector(Vector3 baseDirection, Vector3 projectee)
	{
		baseDirection.Normalize();
		float projecteeMag = projectee.magnitude;
		float projecteeDotBase = Vector3.Dot(projectee / projecteeMag, baseDirection);
		Vector3 projection = baseDirection * projecteeMag * projecteeDotBase;
		return projection;
	}
}
