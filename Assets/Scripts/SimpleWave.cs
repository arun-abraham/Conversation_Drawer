using UnityEngine;
using System.Collections;

public class SimpleWave : MonoBehaviour
{
	public float wavelength;

	public virtual Vector3 FindWavePoint(Vector3 primaryDirection, Vector3 startPoint, float waveStep)
	{
		if (primaryDirection.sqrMagnitude != 1)
		{
			primaryDirection.Normalize();
		}

		Vector3 wavePoint = primaryDirection * wavelength * waveStep;
		wavePoint += startPoint;

		return wavePoint;
	}
}
