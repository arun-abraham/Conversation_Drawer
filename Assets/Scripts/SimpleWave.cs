using UnityEngine;
using System.Collections;

public class SimpleWave : MonoBehaviour
{
	public int maxEstimates = 10;
	public float estimateTolerance;
	public float wavelength;
	

	public virtual Vector3 FindWavePoint(Vector3 primaryDirection, Vector3 startPoint, float time)
	{
		if (primaryDirection.sqrMagnitude != 1)
		{
			primaryDirection.Normalize();
		}

		Vector3 wavePoint = primaryDirection * wavelength * time;
		wavePoint += startPoint;

		return wavePoint;
	}

	public virtual float ApproximateWaveTime(Vector3 primaryDirection, Vector3 startPoint, float arcLength)
	{
		// First estimate is the amount of one wave that has been travelled already.
		float estimateTime = arcLength / wavelength;
		float error = CalcuateError(primaryDirection, startPoint, estimateTime, arcLength);
		for (int i = 0; i < maxEstimates && error > estimateTolerance; i++)
		{
			// Iteratively estimate based on error and rate of change, preventing an estimate from being negative.
			float nextEstimateTime = estimateTime - error / SpeedAtTime(estimateTime);
			if (nextEstimateTime < 0)
			{
				estimateTime = (estimateTime + nextEstimateTime) / 2;
			}
			else
			{
				estimateTime = nextEstimateTime;
			}
			error = CalcuateError(primaryDirection, startPoint, estimateTime, arcLength);
		}
		return estimateTime;
	}

	protected virtual float SpeedAtTime(float time)
	 {
		 return 1;
	 }

	protected virtual float CalcuateError(Vector3 primaryDirection, Vector3 startPoint, float estimateTime, float arcLength)
	{
		return (FindWavePoint(primaryDirection, startPoint, estimateTime) - startPoint).magnitude - arcLength;
	}
}
