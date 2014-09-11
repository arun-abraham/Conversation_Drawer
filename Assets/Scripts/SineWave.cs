using UnityEngine;
using System.Collections;

public class SineWave : SimpleWave {
	public float amplitude;

	public override Vector3 FindWavePoint(Vector3 primaryDirection, Vector3 startPoint, float waveStep)
	{
		if (primaryDirection.sqrMagnitude != 1)
		{
			primaryDirection.Normalize();
		}

		Vector3 wavePoint = primaryDirection * wavelength * waveStep;
		Vector3 transverse = Vector3.Cross(primaryDirection, Vector3.forward) * amplitude;
		transverse *= Mathf.Sin(Mathf.PI * 2 * waveStep);
		wavePoint += transverse;
		wavePoint += startPoint;

		return wavePoint;
	}
}
