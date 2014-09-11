using UnityEngine;
using System.Collections;

public class WaveSeek : MonoBehaviour {
	public Vector3 primaryDirection = Vector3.down;
	public float currentSpeed;
	public float amplitude;
	public float wavelength;
	public float wavePortion;

	void Update()
	{
		if (primaryDirection.sqrMagnitude != 1)
		{
			primaryDirection.Normalize();
		}

		// TODO use speed to determine how far along the curve to move, not just in the primary direction.
		float primaryMove = currentSpeed * Time.deltaTime;
		float step = primaryMove / wavelength;
		wavePortion += step;
		if (wavePortion > 1)
		{
			wavePortion -= 1;
		}
		Debug.Log(wavePortion);
		transform.position += (primaryDirection * primaryMove) + (Vector3.Cross(primaryDirection, Vector3.forward) * amplitude * Mathf.Cos(Mathf.PI * 2 * wavePortion)); 
	}
}
