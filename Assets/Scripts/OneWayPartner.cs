using UnityEngine;
using System.Collections;

public class OneWayPartner : MonoBehaviour {

	private PartnerLink partnerlink;
	private PartnerLink targetPartnerLink;
	private WaypointSeek seeker;
	public GameObject target;
	public bool followTarget = false;
	private bool following = false;


	// Use this for initialization
	void Start () {
		partnerlink = gameObject.GetComponent<PartnerLink>();
		targetPartnerLink = target.GetComponent<PartnerLink>();
		seeker = GetComponent<WaypointSeek>();
	}
	
	// Update is called once per frame
	void Update () {
		if(followTarget && partnerlink.Partner == null){
			partnerlink.SetPartner(targetPartnerLink);
			following = true;
		}
		else if(targetPartnerLink.Partner != partnerlink && !followTarget && following){
			partnerlink.SetPartner(null);
			following = false;
		}
		if(followTarget && following)
			seeker.SeekPartner();
	}
}
