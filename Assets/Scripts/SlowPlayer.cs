using UnityEngine;
using System.Collections;

public class SlowPlayer : MonoBehaviour {

	private bool slowing = false;
	private SimpleMover mover;
	private WaypointSeek seeker;
	private PartnerLink partner;
	private static int otherPartners = 0;
	private float decayRate = 0.9f;

	// Use this for initialization
	void Start () {
		partner = GetComponent<PartnerLink>();
		seeker = GetComponent<WaypointSeek>();
	}
	
	// Update is called once per frame
	void Update () {
		if(seeker == null || mover == null)
			return;
		if(seeker.orbit == true && slowing == false)
		{
			slowing = true;
			mover.externalSpeedMultiplier -= .2f*otherPartners*decayRate;
			otherPartners++;
		}
		else if(slowing == true)
		{
			slowing = false;
			mover.externalSpeedMultiplier += .2f*otherPartners*decayRate;
			otherPartners--;
		}
	}
	void LinkPartner(){
		mover = partner.Partner.GetComponent<SimpleMover>();
	}
}
