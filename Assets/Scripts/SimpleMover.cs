using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour {
	public float maxSpeed;
	public float currentSpeed;
	public float externalMultiplier = 1;
	public bool moving;

	private float movePower = 5;

	void Update() {
		if (!moving) {
			currentSpeed = 0;
		}
	}

	public void Move(Vector3 direction, float speed, bool clampSpeed) {
		if (direction.sqrMagnitude != 1) {
			direction.Normalize();
		}
		if (clampSpeed && speed > maxSpeed) {
			speed = maxSpeed;
		}
		currentSpeed = speed * externalMultiplier;
		//transform.position += direction * currentSpeed * Time.deltaTime;
		rigidbody.AddForce(direction * currentSpeed);

		if(rigidbody.velocity.x > 20)
		{
			rigidbody.velocity = new Vector3(20f, rigidbody.velocity.y, 0);
		}

		if(rigidbody.velocity.x < -20)
		{
			rigidbody.velocity = new Vector3(-20f, rigidbody.velocity.y, 0);
		}

		if(rigidbody.velocity.y > 20)
		{
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, 20f, 0);
		}

		if(rigidbody.velocity.y < -20)
		{
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, -20f, 0);
		}

	


		//Debug.Log(rigidbody.velocity);
	}

	public void SlowDown()
	{
		rigidbody.velocity = rigidbody.velocity * 0.99f;
	}
}
