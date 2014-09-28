using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSeek : SimpleSeek {

	public GameObject geometry;
	[SerializeField]
	public List<Waypoint> waypoints;
	public bool showWaypoints;
	public int current;
	private int previous;
	private bool collideWithWaypoint = false;
	public float partnerWeight;
	public GameObject waypointContainer;
	public bool moveWithoutPartner = false;
	public float maxDesireToLead = 1;
	public float minDesireToLead = 0;
	public float inWakeLeadGrowth = 0.1f;
	public float outWakeLeadGrowth = 0.1f;
	public float desireToLead;
	private bool yielding = false;
	public bool orbit = false;
	public float orbitRadius = 5.0f;
	public bool likesConversation = true;
	
	protected override void Start()
	{
		base.Start();
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

		if (waypointContainer != null)
		{
			Waypoint[] waypointObjects = waypointContainer.GetComponentsInChildren<Waypoint>();
			int startIndex = -1;
			for (int i = 0; i < waypointObjects.Length && startIndex < 0; i++)
			{
				if (waypointObjects[i].isStart)
				{
					startIndex = i;
				}
			}
			if (startIndex >= 0)
			{
				waypoints = new List<Waypoint>();
				while (waypoints.Count < waypointObjects.Length)
				{
					if (startIndex > 0 && startIndex + waypoints.Count >= waypointObjects.Length)
					{
						startIndex = 0;
					}
					waypoints.Add(waypointObjects[startIndex + waypoints.Count]);
				}
			}
		}
		
		SeekNextWaypoint();
		collideWithWaypoint = false;
		if (previous >= 0 && previous < waypoints.Count)
		{
			transform.position = waypoints[previous].transform.position;
		}

		for (int i = 0; i < waypoints.Count; i++)
		{
			waypoints[i].renderer.enabled = showWaypoints;
		}
	}
	
	void Update()
	{
		partnerWeight = Mathf.Clamp(partnerWeight, 0, 1);
		
		if (partnerLink.Partner != null)
		{
			if (partnerLink.Leading)
			{
				Vector3 destination = transform.position;
				if (current < waypoints.Count)
				{
					destination = FindSeekingPoint((waypoints[current].transform.position - transform.position) * mover.maxSpeed);
				}
				
				Vector3 fromPartner = transform.position - partnerLink.Partner.transform.position;
				fromPartner.z = 0;
				Vector3 fromPast = destination - transform.position;
				fromPast.z = 0;
				
				if (partnerWeight > 0 && Vector3.Dot(fromPartner, fromPast) <= 0)
				{
					Vector3 waveFollowChange = fromPast * (1 - partnerWeight);
					
					Vector3 considerPartnerChange = fromPartner.normalized * mover.maxSpeed * partnerWeight * Time.deltaTime;
					destination = transform.position + waveFollowChange + considerPartnerChange;
				}
				
				mover.Accelerate(destination - transform.position);
				if (tracer.lineRenderer == null && tail == null)
				{
					tracer.StartLine();
				}
			}
			else
			{
				if (likesConversation)
				{
					SeekPartner();
				}
				else if(moveWithoutPartner)
				{
					MoveWithoutPartner();
				}
				else
				{
					mover.SlowDown();
				}
			}
		}
		else if (moveWithoutPartner)
		{
			MoveWithoutPartner();
		}
		else
		{
			mover.SlowDown();
			if (tail == null)
			{
				tracer.DestroyLine();
			}
			if (tailTrigger != null)
			{
				tail.trigger.enabled = true;
			}
		}
		geometry.transform.LookAt(transform.position + mover.velocity, geometry.transform.up);

		if (tracer != null)
		{
			if (tail != null)
			{
				tracer.AddVertex(tail.transform.position);
			}
			else
			{
				tracer.AddVertex(transform.position);
			}
		}
	}
	
	public Vector3 FindSeekingPoint(Vector3 velocity)
	{
		if (waypoints == null || waypoints.Count <= 0 || current >= waypoints.Count)
		{
			return transform.position;
		}
		
		Vector3 movement = velocity * Time.deltaTime;
		Vector3 projection = Helper.ProjectVector(waypoints[current].transform.position - waypoints[previous].transform.position, transform.position + movement - waypoints[previous].transform.position);
		
		// If the distance travelled has exceeded the span between the current waypoint, update it.
		if (collideWithWaypoint)
		{
			SeekNextWaypoint();
			collideWithWaypoint = false;
		}
		
		return waypoints[previous].transform.position + projection;
	}
	
	private void SeekNextWaypoint()
	{
		if (waypoints == null || waypoints.Count <= 0 || current >= waypoints.Count)
		{
			return;
		}

		if (showWaypoints)
		{
			waypoints[current].renderer.material.color = new Color(1, 1, 1, 1);
		}

		previous = current;
		collideWithWaypoint = false;

		
		
		// If the node loops back, place the target the waypoint being passed and move all the waypoints to create cycle.
		if (waypoints[previous].loopBackTo != null)
		{
			if (waypoints[previous].maxLoopBacks < 0 || waypoints[previous].maxLoopBacks > waypoints[previous].loopBacks)
			{
				waypoints[previous].loopBacks++;
				Vector3 newStart = waypoints[previous].transform.position;
				int newPrevious = 0;
				for (int i = 0; i < waypoints.Count; i++)
				{
					if (waypoints[i] == waypoints[previous].loopBackTo)
					{
						newPrevious = i;
					}
					else
					{
						Vector3 toNext = waypoints[i].transform.position - waypoints[previous].loopBackTo.transform.position;
						waypoints[i].transform.position = newStart + toNext;
					}
				}
				waypoints[previous].loopBackTo.transform.position = newStart;
				previous = newPrevious;
			}
			else
			{
				waypoints[previous].loopBacks = 0;
			}
			
		}
		current = previous + 1;

		if (showWaypoints)
		{
			waypoints[current].renderer.material.color = new Color(0, 1, 0, 1);
		}
	}

	public override void SeekPartner()
	{
		Mathf.Clamp(desireToLead, 0, 1);
		Vector3 toPartner = partnerLink.Partner.transform.position - transform.position;
		Vector3 toPartnerDestination = toPartner + partnerLink.Partner.mover.velocity;
		if (partnerLink.Yielding)
		{
			mover.Accelerate(partnerLink.Partner.mover.velocity);
		}
		else if(orbit)
		{
			BeginOrbit();
		}
		else if (partnerLink.InWake)
		{
			mover.Accelerate(((1 - desireToLead) * toPartner) + (desireToLead * toPartnerDestination));
			desireToLead += inWakeLeadGrowth * Time.deltaTime;
		}
		else
		{
			mover.Accelerate(toPartner);
			desireToLead += outWakeLeadGrowth * Time.deltaTime;
		}
		if (tracer.lineRenderer == null && tail == null)
		{
			tracer.StartLine();
		}
	}

	private void MoveWithoutPartner()
	{
		Vector3 destination = FindSeekingPoint((waypoints[current].transform.position - transform.position) * mover.maxSpeed);
		mover.Accelerate(destination - transform.position);
		mover.Accelerate(destination - transform.position);
		if (tracer.lineRenderer == null && tail == null)
		{
			tracer.StartLine();
		}
	}

	void BeginOrbit()
	{
		Vector3 fromTarget = transform.position - partnerLink.Partner.transform.position;
		Vector3 destination = Vector3.RotateTowards(fromTarget.normalized * orbitRadius, Vector3.Cross(fromTarget, transform.forward), mover.maxSpeed / orbitRadius * Time.deltaTime, 0);
		mover.Move((partnerLink.Partner.transform.position + destination) - transform.position, mover.maxSpeed);
	}
	
	void OnTriggerEnter(Collider otherCol)
	{
		if (waypoints != null && current < waypoints.Count && otherCol.gameObject == waypoints[current].gameObject)
		{
			collideWithWaypoint = true;
		}
	}

	private void StartLeading()
	{
		desireToLead = minDesireToLead;
		Vector3 waypointOffset = transform.position - waypoints[previous].transform.position;
		for (int i = 0; i < waypoints.Count; i++)
		{
			waypoints[i].transform.position += waypointOffset;
		}
	}

	private void StartYielding()
	{
		yielding = true;
	}

	private void TailStartFollow()
	{
		if (tracer != null)
		{
			tracer.StartLine();
		}
		if (tailTrigger != null)
		{
			tail.trigger.enabled = false;
		}
	}
	
	private void TailEndFollow()
	{
		if (tracer)
		{
			tracer.DestroyLine();
		}
	}

	private void LinkPartner()
	{
		likesConversation = true;
	}

	private void MinMaxSpeedReached()
	{
		likesConversation = false;
	}
}
