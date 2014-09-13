using UnityEngine;
using System.Collections;

public class FollowScore : MonoBehaviour {
	public SimpleMover mover;
	public WaveSeek leaderMover;
	public Tracer tracer;
	public Tracer leader;
	public int oldNearestIndex = 0;
	public float followThreshold;
	public float score = 0;
	public float perfectScoreIncrement;
	public float scoreLimit;
	public float scoreLimitBoost;
	public bool forcingScore = false;
	public float scoreDecayTime;
	private float startSpeed;
	public bool allowBoosts = true;
	public Camera gameCamera = null;

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		startSpeed = mover.maxSpeed;
	}

	void Update()
	{
		if (!mover.Moving)
		{
			SendMessage("SpeedNormal", SendMessageOptions.DontRequireReceiver);
		}
		else if (leader.GetVertexCount() > 1 || tracer.GetVertexCount() > 1)
		{
			// Find nearest vertex on leader line and the vertex after it.
			int nearestIndex = leader.FindNearestIndex(transform.position, oldNearestIndex);
			Vector3 nearestVertex = leader.GetVertex(nearestIndex);
			Vector3 nextVertex;
			if (nearestIndex < leader.GetVertexCount() - 1)
			{
				nextVertex = leader.GetVertex(nearestIndex + 1);
			}
			else
			{
				nextVertex = nearestVertex;
				nearestVertex = leader.GetVertex(nearestIndex - 1);
			}

			// Compare follower to leader line.
			Vector3 nearestToNext = (nextVertex - nearestVertex).normalized;
			Vector3 nearestToFollower = (transform.position - nearestVertex);
			Vector3 pointOnPath = Helper.ProjectVector(nearestToNext, nearestToFollower) + nearestVertex;
			float followerToPathDist = (transform.position - pointOnPath).magnitude;			
			
			if (!forcingScore)
			{
				float scoreFactor = Mathf.Max(1 - (followerToPathDist / followThreshold), -1);
				score += perfectScoreIncrement * scoreFactor * Time.deltaTime;
			}
			else
			{
				float decayPortion = Time.deltaTime / scoreDecayTime;
				
				if (score > 0)
				{
					score -= Mathf.Min(decayPortion * scoreLimit, score);
					mover.maxSpeed += scoreLimitBoost * decayPortion;
				}
				else if (score < 0)
				{
					score += Mathf.Min(decayPortion * scoreLimit, -score);
					mover.maxSpeed -= scoreLimitBoost * decayPortion;
				}
				else
				{
					SendMessage("SpeedNormal", SendMessageOptions.DontRequireReceiver);
					forcingScore = false;
					//leaderMover.currentSpeed += scoreLimitBoost;
					// TODO Should we update the leader's speed instead?
					mover.maxSpeed = startSpeed;
				}
			}

			if (allowBoosts)
			{
				if (score >= scoreLimit && !forcingScore)
				{
					forcingScore = true;
					SendMessage("SpeedBoost", SendMessageOptions.DontRequireReceiver);
				}
				else if (score <= -scoreLimit && !forcingScore)
				{
					forcingScore = true;
					SendMessage("SpeedDrain", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
