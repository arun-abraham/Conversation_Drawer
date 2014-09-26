using UnityEngine;
using System.Collections;

public class GlobalPoints : MonoBehaviour {

	public GameObject currentPoints = null;
	private int i;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		i = transform.childCount - 1;
	
	}

	void EndLeading()
	{
		
		//createdPoints.SendMessage("IsFading",SendMessageOptions.DontRequireReceiver);
		//Invoke("DestroyPoints",5.0f);
		//print ("is uncoupled");
		//currentPoints = null;
	}
	
	void PointsFade()
	{
		transform.GetChild(i).gameObject.SendMessage("IsFading",SendMessageOptions.DontRequireReceiver);
	
	}
	
	void PointsBright()
	{
		transform.GetChild(i).gameObject.SendMessage("IsBright",SendMessageOptions.DontRequireReceiver);
	
	}
	
	void  UnlinkPartner()
	{
		DestroyPoints();
	}
	
	private void DestroyPoints()
	{  
		Destroy(transform.GetChild(i).gameObject);
	
	}
}
