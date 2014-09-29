using UnityEngine;
using System.Collections;

public class SlowPlayer : MonoBehaviour {

	private bool slowing = false;
	private SimpleMover mover;
	private WaypointSeek seeker;
	private PartnerLink partnerLink;
	private static int otherPartners = 0;
	private float decayRate = 0.9f;
	public float slowRate = 0.2f;

	// Use this for initialization
	void Start () {
		partnerLink = GetComponent<PartnerLink>();
		seeker = GetComponent<WaypointSeek>();
	}
	
	// Update is called once per frame
	void Update () {
		if(seeker == null || mover == null)
			return;
		if(seeker.orbit == true && slowing == false)
		{
			slowing = true;
			otherPartners++;
			mover.externalSpeedMultiplier -= slowRate * otherPartners * decayRate;
		}
		else if(seeker.orbit == false && slowing == true)
		{
			slowing = false;
			mover.externalSpeedMultiplier += slowRate*otherPartners*decayRate;
			otherPartners--;
		}
	}

	void LinkPartner(){
		if (partnerLink.Partner != null)
		{
			mover = partnerLink.Partner.GetComponent<SimpleMover>();
		}
	}
}
