using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {



	//For Testing:


	/*public bool shakingMain = false;
	
	// Update is called once per frame
	void Update () {

		ShakeCamera(shakingMain, .7f);
	
	}*/


	public void ShakeCamera(bool shaking, float shakeAmount)
	{
		if(shaking)
		{
			camera.transform.localPosition = Random.insideUnitSphere * shakeAmount;
			camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, camera.transform.localPosition.y, -10f);
		}
	}
	
}
