using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSeek : MonoBehaviour {
	
	[SerializeField]
	public List<Waypoint> waypoints;
	public bool showWaypoints;
	public int current;
	private int previous;
	private bool collideWithWaypoint = false;
	public SimpleMover mover;
	public PartnerLink partnerLink;
	public Tracer tracer;
	private bool startedLine = false;
	public float partnerWeight;
	public GameObject waypointContainer;
	public bool moveWithoutPartner = false;
	
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
			if (tracer != null && !startedLine)
			{
				tracer.StartLine();
				startedLine = true;
			}

			if (partnerLink.Leading)
			{
				Vector3 destination = FindSeekingPoint((waypoints[current].transform.position - transform.position) * mover.maxSpeed);
				
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
				
				if (tracer != null)
				{
					tracer.AddVertex(transform.position);
				}
			}
			else
			{
				mover.Accelerate(partnerLink.Partner.transform.position - transform.position);
				if (tracer != null)
				{
					tracer.AddVertex(transform.position);
				}
			}
		}
		else if (moveWithoutPartner)
		{
			if (tracer != null && !startedLine)
			{
				tracer.StartLine();
				startedLine = true;
			}
			Vector3 destination = FindSeekingPoint((waypoints[current].transform.position - transform.position) * mover.maxSpeed);
			mover.Accelerate(destination - transform.position);
			mover.Accelerate(destination - transform.position);
			if (tracer != null)
			{
				tracer.AddVertex(transform.position);
			}
		}
		else
		{
			mover.SlowDown();
			if (tracer)
			{
				tracer.DestroyLine();
				startedLine = false;
			}
		}
	}
	
	public Vector3 FindSeekingPoint(Vector3 velocity)
	{
		if (waypoints == null || waypoints.Count <= 0 && current >= waypoints.Count)
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
		if (waypoints == null || waypoints.Count <= 0)
		{
			return;
		}
		
		previous = current;
		
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
	}
	
	void OnTriggerEnter(Collider otherCol)
	{
		if (waypoints != null && current < waypoints.Count && otherCol.gameObject == waypoints[current].gameObject)
		{
			collideWithWaypoint = true;
		}
	}
}
