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
	private bool yielding;
	public bool Yielding
	{
		get { return yielding; }
		set 
		{
			if (value != yielding)
			{
				yielding = value; 
				if (yielding == true)
				{
					SendMessage("StartYielding", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					SendMessage("EndYielding", SendMessageOptions.DontRequireReceiver);
				}
			}
			
		}
	}
	public float startYieldProximity = 1;
	public float endYieldProximity = 2;
	public float yieldSpeedModifier = -0.5f;
	public float timeToOvertake = 3;
	public float timeToYield = 3;
	public bool inWake = false;
	public bool InWake
	{
		get { return inWake; }
		set
		{
			if (value != inWake)
			{
				inWake = value;
				if (inWake == true)
				{
					SendMessage("EnterWake", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					SendMessage("ExitWake", SendMessageOptions.DontRequireReceiver);
				}
			}

		}
	}

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
				PartnerLink potentialPartner = potentials[i].GetComponent<PartnerLink>();
				Conversation potentionalConversation = ConversationManger.Instance.FindConversation(this, potentialPartner);
				if (potentionalConversation != null && potentials[i] != gameObject && (transform.position - potentials[i].transform.position).sqrMagnitude <= Mathf.Pow(potentionalConversation.initiateDistance, 2))
				{
					ConversationManger.Instance.StartConversation(this, potentialPartner);
				}
			}
		}

		// Handle partners seperating.
		if (partner != null && seekingPartner)
		{
			// Show that partners are close to separating.
			float sqrDist = (transform.position - partner.transform.position).sqrMagnitude;
			if (sqrDist > Mathf.Pow(conversation.breakingDistance, 2))
			{
				ConversationManger.Instance.EndConversation(this, partner);
				if (partnerLine != null)
				{
					partnerLine.SetVertexCount(0);
				}
			}
			else if (sqrDist > Mathf.Pow(conversation.warningDistance, 2))
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

	public bool ShouldLead(PartnerLink leader)
	{
		Vector3 toLeader = leader.transform.position - transform.position;
		bool far = toLeader.sqrMagnitude >= Mathf.Pow(leader.startYieldProximity, 2);
		bool behind = Vector3.Dot(toLeader, leader.mover.velocity) >= 0;
		return !far || !behind;
	}

	public bool ShouldYield(PartnerLink leader)
	{
		Vector3 toLeader = leader.transform.position - transform.position;
		bool far = toLeader.sqrMagnitude >= Mathf.Pow(leader.startYieldProximity + endYieldProximity, 2);
		bool behind = Vector3.Dot(toLeader, leader.mover.velocity) >= 0;
		return !far || !behind;
	}
}
