using UnityEngine;
using System.Collections;

public class PartnerLink : MonoBehaviour {
	private PartnerLink partner;
	public PartnerLink Partner
	{
		get { return partner; }
	}
	private Conversation conversation;
	public Conversation Conversation
	{
		get { return conversation; }
	}
	private bool leading;
	public bool Leading
	{
		get { return leading; }
	}
	public bool seekingPartner;
	public float converseDistance;
	public float warningThreshold;
	public float breakingThreshold;
	private LineRenderer partnerLine;
	public float partnerLineSize = 0.25f;
	private float partnerLineAlteredSize;
	public float partnerLineShrink = 0.9f;
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
						ConversationManger.Instance.StartConversation(this, potentialPartner);
					}
				}
			}
		}

		// Handle partners seperating.
		if (partner != null && seekingPartner)
		{
			// Show that partners are close to separating.
			float sqrDist = (transform.position - partner.transform.position).sqrMagnitude;
			if (sqrDist > Mathf.Pow(converseDistance * breakingThreshold, 2))
			{
				ConversationManger.Instance.EndConversation(this, partner);
				if (partnerLine != null)
				{
					partnerLine.SetVertexCount(0);
				}
			}
			else if (sqrDist > Mathf.Pow(converseDistance * warningThreshold, 2))
			{
				if (partnerLine != null)
				{
					partnerLine.SetWidth(partnerLineSize, partnerLineSize);
					partnerLineAlteredSize = partnerLineSize;
					partnerLine.SetVertexCount(2);
					partnerLine.SetPosition(0, transform.position);
					partnerLine.SetPosition(1, partner.transform.position);
				}
			}
			else
			{
				if (partnerLine != null)
				{
					partnerLineAlteredSize *= partnerLineShrink;
					partnerLine.SetWidth(partnerLineAlteredSize, partnerLineAlteredSize);
					partnerLine.SetVertexCount(2);
					partnerLine.SetPosition(0, transform.position);
					partnerLine.SetPosition(1, partner.transform.position);
					if (partnerLineAlteredSize / partnerLineSize < (1 - partnerLineShrink))
					{
						partnerLine.SetVertexCount(0);
					}
				}
			}
		}
	}

	public void SetPartner(PartnerLink partner)
	{
		this.partner = partner;

		if (partner != null)
		{
			conversation = ConversationManger.Instance.FindConversation(this, partner);
			SendMessage("LinkPartner", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			conversation = null;
			SendMessage("UnlinkPartner", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void SetLeading(bool isLead, bool updatePartner = true)
	{
		leading = isLead;
		if (isLead)
		{
			if (conversation.partner1 == this)
			{
				conversation.partner1Leads = true;
			}
			else
			{
				conversation.partner1Leads = false;
			}
			SendMessage("StartLeading", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			SendMessage("EndLeading", SendMessageOptions.DontRequireReceiver);
		}
		if (partner != null && updatePartner)
		{
			partner.SetLeading(!isLead, false);
		}
	}
}
