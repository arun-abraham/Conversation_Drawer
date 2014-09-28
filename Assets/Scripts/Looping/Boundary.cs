using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {

	public TriggerLooping.ColliderLocation colliderLocation;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Converser")
		{
			if(other.transform == transform.parent.GetComponent<TriggerLooping>().player.transform)
				transform.parent.GetComponent<TriggerLooping>().MoveWorld(colliderLocation);
			else
				transform.parent.GetComponent<TriggerLooping>().MoveIndividual(colliderLocation, other.gameObject);

		}
	}

}
