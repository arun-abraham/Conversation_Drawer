using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
	public bool isStart = false;
	public Waypoint loopBackTo;
	public int maxLoopBacks;
	public int loopBacks;
}
