﻿using UnityEngine;
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
	public float partnerWeight;

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
		if (tracer)
		{
			tracer.StartLine();
		}
		for (int i = 0; i < waypoints.Count; i++)
		{
			waypoints[i].renderer.enabled = showWaypoints;
		}
		SeekNextWaypoint();
		transform.position = waypoints[previous].transform.position;
	}

	void Update()
	{
		partnerWeight = Mathf.Clamp(partnerWeight, 0, 1);

		if (partnerLink.Partner != null)
		{
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
	}

	public Vector3 FindSeekingPoint(Vector3 velocity)
	{
		if (current >= waypoints.Count)
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
		if (otherCol.gameObject == waypoints[current].gameObject)
		{
			collideWithWaypoint = true;
		}
	}
}
