using UnityEngine;
using System.Collections;

public class Tail : MonoBehaviour {
	public PartnerLink partnerLink;
	public SimpleMover mover;
	private bool following = false;
	public Collider trigger;

	void Awake()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (trigger == null)
		{
			trigger = partnerLink.collider;
		}
	}

	void Start()
	{
		Vector3 localScale = transform.localScale;
		localScale.y = partnerLink.tracer.trailNearWidth;
		transform.localScale = localScale;
	}
	
	void Update()
	{
		// Set speed proportional to head's speed to slowly wait for yield proximity.
		Vector3 fromHead = (transform.position - partnerLink.transform.position);
		float yieldProximityPortion = fromHead.magnitude / partnerLink.startYieldProximity;
		mover.maxSpeed = partnerLink.mover.maxSpeed * yieldProximityPortion / 2;
		mover.Move(partnerLink.mover.transform.position - transform.position, mover.maxSpeed);

		if (following)
		{
			// If tail can be turned off, fade as it approaches trigger.
			if (trigger.enabled)
			{
				Color color = renderer.material.color;
				color.a = yieldProximityPortion;
				if (partnerLink.tracer.lineRenderer != null)
				{
					Color nearColor = renderer.material.color;
					nearColor.a = Mathf.Min(yieldProximityPortion, 1);
					Color farColor = renderer.material.color;
					farColor.a = 0;
					partnerLink.tracer.lineRenderer.SetColors(farColor, nearColor);
					partnerLink.tracer.lineRenderer.SetWidth(((1 - yieldProximityPortion) * partnerLink.tracer.trailNearWidth) + (yieldProximityPortion * partnerLink.tracer.trailFarWidth), partnerLink.tracer.trailNearWidth);
				}
			}

			// Render and align to movement.
			renderer.enabled = true;
			renderer.material.color = partnerLink.renderer.material.color;

			if (fromHead.sqrMagnitude > 0)
			{
				transform.rotation = Quaternion.LookRotation(fromHead, Vector3.up);
			}
		}
		else
		{
			renderer.enabled = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other == trigger)
		{
			following = false;
			SendEndFollow();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other == trigger)
		{
			following = true;
			SendStartFollow();
		}
	}

	private void SendStartFollow()
	{
		SendMessage("TailStartFollow", SendMessageOptions.DontRequireReceiver);
		partnerLink.SendMessage("TailStartFollow", SendMessageOptions.DontRequireReceiver);
	}

	private void SendEndFollow()
	{
		SendMessage("TailEndFollow", SendMessageOptions.DontRequireReceiver);
		partnerLink.SendMessage("TailEndFollow", SendMessageOptions.DontRequireReceiver);
	}
}
