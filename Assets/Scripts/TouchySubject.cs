using UnityEngine;
using System.Collections;

public class TouchySubject : MonoBehaviour {

	private SimpleMover mover;
	private float originalMaxSpeed;
	private bool cooldown = false;
	public float cooldownTimer = 0.3f;
	private float timer;
	private CursorSeek cursor;

	// Use this for initialization
	void Start () {
		mover = GetComponent<SimpleMover>();
		cursor = GetComponent<CursorSeek>();
		originalMaxSpeed = mover.maxSpeed;
		timer = cooldownTimer;
	}
	
	// Update is called once per frame
	void Update () {
		if(cooldown)
		{
			timer -= Time.deltaTime;
			if(timer <= 0)
			{
				timer = cooldownTimer;
				cooldown = false;
				cursor.enabled = true;
				mover.maxSpeed = originalMaxSpeed;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag != "TouchySubject")
			return;

		originalMaxSpeed = mover.maxSpeed;
		Vector3 pushDirection = transform.position - col.gameObject.transform.position;
		mover.maxSpeed = 50;
		mover.Move(pushDirection, mover.maxSpeed);
		cursor.enabled = false;
	//	rigidbody.AddForce(transform.forward * 100, ForceMode.VelocityChange);
		if(!cooldown)
			cooldown = true;

	}
}
