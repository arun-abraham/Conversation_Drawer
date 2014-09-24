using UnityEngine;
using System.Collections;

public class LoopTag : MonoBehaviour {

	//Will the object move if it's on screen?
	public bool loopOnCamera = false;

	//Move the parent object or just the child?
	public bool moveRoot = true;

	public bool boundaryCollider = false;
}
