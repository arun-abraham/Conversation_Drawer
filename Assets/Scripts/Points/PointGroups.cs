using UnityEngine;
using System.Collections;

public class PointGroups : MonoBehaviour {

	public bool imCreated = false;

	public GameObject myParent;

	// Use this for initialization
	void Start () {

		//gameObject.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(myParent.GetComponent<CastPoints>().isCreated)
		{
			transform.position = new Vector3(0,0,0);

		}
	
	}
}
