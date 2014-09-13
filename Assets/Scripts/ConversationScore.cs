using UnityEngine;
using System.Collections;

public class ConversationScore : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	public Tracer tracer;
	public Tracer partnerTracer;
	public int oldNearestIndex = 0;
	public float followThreshold;
	public float score = 0;
	public float perfectScoreIncrement;
	public float scoreLimit;
	public float scoreLimitBoost;
	public bool changingBoost = false;
	public float scoreDecayTime;
	public float scoreDecayed;
	private float startSpeed;
	public int boostLevels;
	public int currentBoostLevel = 0;
	public Camera gameCamera = null;

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		startSpeed = mover.maxSpeed;
	}
	
	void Update()
	{
		if (!mover.Moving || partnerLink.Partner == null)
		{
			SendMessage("SpeedNormal", SendMessageOptions.DontRequireReceiver);
		}
		else if (partnerTracer.GetVertexCount() > 1 && tracer.GetVertexCount() > 1)
		{
			// Find nearest vertex on leader line and the vertex after it.
			int nearestIndex = partnerTracer.FindNearestIndex(transform.position, oldNearestIndex);
			Vector3 nearestVertex = partnerTracer.GetVertex(nearestIndex);
			Vector3 nextVertex;
			if (nearestIndex < partnerTracer.GetVertexCount() - 1)
			{
				nextVertex = partnerTracer.GetVertex(nearestIndex + 1);
			}
			else
			{
				nextVertex = nearestVertex;
				nearestVertex = partnerTracer.GetVertex(nearestIndex - 1);
			}

			// Compare follower to leader line.
			Vector3 nearestToNext = (nextVertex - nearestVertex).normalized;
			Vector3 nearestToFollower = (transform.position - nearestVertex);
			Vector3 pointOnPath = Helper.ProjectVector(nearestToNext, nearestToFollower) + nearestVertex;
			float followerToPathDist = (transform.position - pointOnPath).magnitude;			
			
			if (!changingBoost)
			{
				float scoreFactor = Mathf.Max(1 - (followerToPathDist / followThreshold), -1);
				score += perfectScoreIncrement * scoreFactor * Time.deltaTime;
			}
			else
			{
				scoreDecayed += Time.deltaTime;
				if (scoreDecayed >= scoreDecayTime)
				{
					SendMessage("SpeedNormal", SendMessageOptions.DontRequireReceiver);
					changingBoost = false;
					scoreDecayed = 0;
				}
			}
			
			float scoreReq = scoreLimit * (1 -((partnerTracer.transform.position - transform.position).magnitude / partnerLink.breakingThreshold));
				
			// Boost speed if score exceeds requirement.
			if (score >= scoreReq && !changingBoost)
			{
				mover.maxSpeed = startSpeed + scoreLimitBoost;
			}
			else
			{
				mover.maxSpeed = startSpeed;
			}

			// Update boost levels 
			if (boostLevels > 0)
			{
				if (score > (scoreLimit / boostLevels) * (currentBoostLevel + 1))
				{
					currentBoostLevel++;
					changingBoost = true;
					SendMessage("SpeedBoost", SendMessageOptions.DontRequireReceiver);
					
				}
				else if (score < (scoreLimit / boostLevels) * currentBoostLevel)
				{
					currentBoostLevel--;
					changingBoost = true;
					SendMessage("SpeedDrain", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	private void LinkPartner()
	{
		if (partnerLink != null && partnerLink.Partner != null)
		{
			partnerTracer = partnerLink.Partner.tracer;
		}
	}

	private void UnlinkPartner()
	{
		partnerTracer = null;
	}
}
