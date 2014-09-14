using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour {

	public bool n = false;
	public bool s = false;
	public bool e = false;
	public bool w = false;
	public bool ne = false;
	public bool se = false;
	public bool sw = false;
	public bool nw = false;


	public float maxSpeed;
	public Vector3 velocity;
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


	// coordinates
		if(velocity.x > 0 && velocity.y == 0)
			e = true;
		else
			e = false;

		if(velocity.x < 0 && velocity.y == 0)
			w = true;
		else
			w = false;

		if(velocity.x == 0 && velocity.y > 0)
			n = true;
		else
			n = false;

		if(velocity.x == 0 && velocity.y < 0)
			s = true;
		else
			s = false;

		if(velocity.x > 0 && velocity.y > 0)
			ne = true;
		else
			ne = false;

		if(velocity.x > 0 && velocity.y < 0)
			se = true;
		else
			se = false;

		if(velocity.x < 0 && velocity.y < 0)
			sw = true;
		else
			sw = false;

		if(velocity.x < 0 && velocity.y > 0)
			nw = true;
		else
			nw = false;
		//

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
