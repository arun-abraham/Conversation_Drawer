using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointWave : SimpleWave {
	[SerializeField]
	List<Waypoint> waypoints;
	public int current;
	private int previous;
	private float travelToPrevious = 0;
	private float prevToCurDist;

	void Start()
	{
		previous = current - 1;
		transform.position = waypoints[current].transform.position;
	}

	public override Vector3 FindWavePoint(Vector3 primaryDirection, Vector3 startPoint, float time)
	{
		if (time < 0)
		{
			return waypoints[previous].transform.position;
		}

		Vector3 wavePoint = waypoints[previous].transform.position + ((waypoints[current].transform.position - waypoints[previous].transform.position) * time);
		return wavePoint;
	}

	public override float ApproximateWaveTime(Vector3 primaryDirection, Vector3 startPoint, float arcLength)
	{
		if (current >= waypoints.Count)
		{
			return -1;
		}

		// If the distance travelled has exceeded the span between the current waypoint, update it.
		if (arcLength - travelToPrevious >= prevToCurDist)
		{
			travelToPrevious += prevToCurDist;
			previous = current;

			// If the node loops back, place the target the waypoint being passed and move all the waypoints to create cycle.
			if (waypoints[previous].loopBackTo != null && (waypoints[previous].maxLoopBacks < 0 || waypoints[previous].maxLoopBacks > waypoints[previous].loopBacks))
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
						waypoints[i].transform.position =newStart + toNext;					
					}
				}
				waypoints[previous].loopBackTo.transform.position = newStart;
				previous = newPrevious;
			}
			current = previous + 1;

			// Calculate distance from previous waypoint to the one sought now.
			if (current < waypoints.Count)
			{
				prevToCurDist = (waypoints[current].transform.position - waypoints[previous].transform.position).magnitude;
			}
			else
			{
				prevToCurDist = 0;
			}
		}

		if (prevToCurDist <= 0)
		{
			return -1;
		}

		return (Mathf.Max(arcLength, travelToPrevious) - travelToPrevious) / prevToCurDist;
	}
}