using UnityEngine;
using System.Collections;

public class SlowPlayer : MonoBehaviour {

	private bool slowing = false;
	private SimpleMover mover;
	private WaypointSeek seeker;
	private PartnerLink partner;

	// Use this for initialization
	void Start () {
		partner = GetComponent<PartnerLink>();
		mover = partner.Partner.GetComponent<SimpleMover>();
		seeker = GetComponent<WaypointSeek>();
	}
	
	// Update is called once per frame
	void Update () {
		if(seeker.orbit == true && slowing == false)
		{
			slowing = true;
			mover.externalSpeedMultiplier -= .2f;
		}
		else if(slowing == true)
		{
			slowing = false;
			mover.externalSpeedMultiplier += .2f;
		}
	}
}
