using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour {
	public float maxSpeed;
	public Vector3 velocity;
	public float acceleration;
	public float handling;
	public float dampening = 0.9f;
	public float dampeningThreshold;
	public float externalSpeedMultiplier = 1;
	private bool moving;
	public bool Moving
	{
		get { return moving; }
	}

	void Update() {
		transform.position += velocity * Time.deltaTime;
		if (velocity.sqrMagnitude < Mathf.Pow(dampeningThreshold, 2)) {
			velocity = Vector3.zero;
			moving = false;
		}
		else
		{
			moving = true;
		}
	}

	public void Accelerate(Vector3 direction) {
		if (direction.sqrMagnitude != 1)
		{
			direction.Normalize();
		}

		if (velocity.sqrMagnitude <= 0) 
		{
			velocity += direction * acceleration * Time.deltaTime;
		}
		else 
		{
			Vector3 parallel = Helper.ProjectVector(velocity, direction);
			Vector3 perpendicular = direction - parallel;

			//if (velocity.sqrMagnitude >= Mathf.Pow(maxSpeed, 2))
			//{
			//	velocity += perpendicular * handling * Time.deltaTime;
			//}
			//else
			//{
				velocity += ((parallel * acceleration) + (perpendicular * handling)) * Time.deltaTime;
			//}
		}

		if (velocity.sqrMagnitude > Mathf.Pow(maxSpeed, 2))
		{
			velocity = velocity.normalized * maxSpeed;
		}
		velocity *= externalSpeedMultiplier;
	}

	public void Move(Vector3 direction, float speed, bool clampSpeed)
	{
		if (direction.sqrMagnitude != 1)
		{
			direction.Normalize();
		}
		if (clampSpeed && speed > maxSpeed)
		{
			speed = maxSpeed;
		}
		velocity = direction * speed * externalSpeedMultiplier;
		transform.position += velocity * Time.deltaTime;
	}

	public void MoveTo(Vector3 position, bool updateVelocity = false)
	{
		if (updateVelocity && Time.deltaTime > 0)
		{
			velocity = (position - transform.position) / Time.deltaTime;
		}
		transform.position = position;
	}

	public void SlowDown()
	{
		velocity *= dampening;
	}
}
