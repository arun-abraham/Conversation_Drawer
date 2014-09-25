using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoolEvent : MonoBehaviour {
	[SerializeField]
	List<GameObject> objectsToShow;

	public void BeCool()
	{
		if (objectsToShow != null)
		{
			for(int i = 0; i < objectsToShow.Count; i++)
			{
				objectsToShow[i].SetActive(true);
			}
		}
			
	}
}
