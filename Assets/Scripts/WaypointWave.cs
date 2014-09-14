using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointWave : SimpleWave {
	[SerializeField]
	List<Waypoint> waypoints;
	public int current;
	private int previous;
	private float travelToPrevious = 0;
	private float prevToCurDist = 0;

	void Start()
	{
		SeekNextWaypoint();
		transform.position = waypoints[previous].transform.position;
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

		// Ignore given arc length, and determine it based on travel between current and previous waypoints.
		float projection = Helper.ProjectVector(waypoints[current].transform.position - waypoints[previous].transform.position, transform.position - waypoints[previous].transform.position).magnitude;
		projection += arcLength;

		if (prevToCurDist <= 0)
		{
			return -1;
		}

		float time = projection / prevToCurDist;

		// If the distance travelled has exceeded the span between the current waypoint, update it.
		if (time > 1)
		{
			SeekNextWaypoint();
			time -= 1;
		}

		arcResetable = true;
		return time;
	}

	private void SeekNextWaypoint()
	{
		travelToPrevious += prevToCurDist;
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
}