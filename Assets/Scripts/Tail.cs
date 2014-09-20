using UnityEngine;
using System.Collections;

public class Tail : MonoBehaviour {
	public GameObject tail;
	public PartnerLink partnerLink;

	void Start()
	{
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
	}
	
	void Update()
	{
		tail.transform.position = transform.position - partnerLink.mover.velocity.normalized * partnerLink.startYieldProximity;
		if ((transform.position - tail.transform.position).sqrMagnitude > 0)
		{
			tail.renderer.enabled = true;
			tail.renderer.material.color = renderer.material.color;
			tail.transform.rotation = Quaternion.LookRotation(transform.position - tail.transform.position);
		}
		else
		{
			tail.renderer.enabled = false;
		}
	}
}
