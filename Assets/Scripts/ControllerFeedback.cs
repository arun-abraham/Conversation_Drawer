using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ControllerFeedback : MonoBehaviour {

	float duration;
	float intensity;
	float startTime;

	// Use this for initialization
	void Start () {
		intensity = 0.0f;
		duration = 3.0f;

	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - startTime < duration)
		{
			GamePad.SetVibration(0, intensity, intensity);
		}
	}

	void HardVibrate (float time) {
		startTime = Time.time;
		intensity = 0.5f;
		duration = 0.5f;
	}
}
