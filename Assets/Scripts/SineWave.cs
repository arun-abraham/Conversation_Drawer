using UnityEngine;
using System.Collections;

public class SineWave : SimpleWave {
	public float amplitude;

	public override Vector3 FindWavePoint(Vector3 primaryDirection, Vector3 startPoint, float time)
	{
		if (primaryDirection.sqrMagnitude != 1)
		{
			primaryDirection.Normalize();
		}

		float componentDistance = Mathf.Pow(wavelength * time, 2) / (amplitude + 1);
		Vector3 wavePoint = primaryDirection * componentDistance;
		Vector3 transverse = Vector3.Cross(primaryDirection, Vector3.forward) * componentDistance * amplitude;
		transverse *= Mathf.Sin(Mathf.PI * 2 * time);
		wavePoint += transverse;
		wavePoint += startPoint;

		if (time >= 1)
		{
			arcResetable = true;
		}

		return wavePoint;
	}

	protected override float SpeedAtTime(float time)
	{
		return Mathf.Cos(Mathf.PI * 2 * time);
	}
}
