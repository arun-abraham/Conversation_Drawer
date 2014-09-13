using UnityEngine;
using System.Collections;

public class PlayerVFX : MonoBehaviour {
	public CameraShake cameraShake;
	public float cameraShakeFactor;

	void Start()
	{
		if (cameraShake == null)
		{
			cameraShake = Camera.main.GetComponent<CameraShake>();
		}
	}

	void SpeedNormal()
	{
		if (cameraShake != null)
		{
			cameraShake.StopShaking();
		}
	}
	void SpeedBoost()
	{
		if (cameraShake != null)
		{
			cameraShake.ShakeCamera(cameraShakeFactor);
		}
	}

	void SpeedDrain()
	{

	}

}
