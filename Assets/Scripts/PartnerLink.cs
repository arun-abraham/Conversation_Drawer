using UnityEngine;
using System.Collections;

public class PartnerLink : MonoBehaviour {
	private PartnerLink partner;
	public PartnerLink Partner
	{
		get { return partner; }
	}
	public bool seekingPartner;
	public float converseDistance;
	public float warningThreshold;
	public float breakingThreshold;
	public LineRenderer partnerLine;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	[HideInInspector]
	public ConversationScore conversationScore;


	void Awake()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		if (conversationScore == null)
		{
			conversationScore = GetComponent<ConversationScore>();
		}

		partnerLine = GetComponent<LineRenderer>();
	}

	void Update()
	{
		// Find a partner.
		if (seekingPartner && partner == null)
		{
			GameObject[] potentials = GameObject.FindGameObjectsWithTag("Converser");
			for (int i = 0; i < potentials.Length; i++)
			{
				if (potentials[i] != gameObject && (transform.position - potentials[i].transform.position).sqrMagnitude <= Mathf.Pow(converseDistance, 2))
				{
					PartnerLink potentialPartner = potentials[i].GetComponent<PartnerLink>();
					if (potentialPartner != null)
					{
						SetPartner(potentialPartner);
					}
				}
			}
		}

		// Handle partners seperating.
		if (partner != null)
		{
			// Show that partners are close to separating.
			float sqrDist = (transform.position - partner.transform.position).sqrMagnitude;
			if (sqrDist > Mathf.Pow(converseDistance * breakingThreshold, 2))
			{
				SetPartner(null);
				if (partnerLine != null)
				{
					partnerLine.SetVertexCount(0);
				}
			}
			else if (sqrDist > Mathf.Pow(converseDistance * warningThreshold, 2))
			{
				if (partnerLine != null)
				{
					partnerLine.SetVertexCount(2);
					partnerLine.SetPosition(0, transform.position);
					partnerLine.SetPosition(1, partner.transform.position);
				}
			}
		}
	}

	public void SetPartner(PartnerLink partner, bool updatePartner = true)
	{
		if (updatePartner && this.partner != null && this.partner.partner == this)
		{
			this.partner.SetPartner(null, false);
		}

		this.partner = partner;

		if (partner != null)
		{
			SendMessage("LinkPartner", SendMessageOptions.DontRequireReceiver);
			if (updatePartner)
			{
				partner.SetPartner(this, false);
			}
		}
		else
		{
			SendMessage("UnlinkPartner", SendMessageOptions.DontRequireReceiver);
		}
	}
}
