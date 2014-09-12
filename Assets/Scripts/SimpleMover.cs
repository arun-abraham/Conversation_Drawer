using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour {
	public float maxSpeed;
	public Vector3 velocity;
	public float dampening = 0.9f;
	public float externalSpeedMultiplier = 1;
	private bool moving;
	public bool Moving
	{
		get { return moving; }
	}

	void Update() {
		transform.position += velocity * Time.deltaTime;
		if (velocity.sqrMagnitude < 0.0001f) {
			velocity = Vector3.zero;
			moving = false;
		}
		else
		{
			moving = true;
		}
	}

	public void Accelerate(Vector3 acceleration) {
		velocity += acceleration * Time.deltaTime;
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

	public void SlowDown()
	{
		velocity *= dampening;
	}
}
