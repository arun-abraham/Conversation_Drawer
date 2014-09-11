using UnityEngine;
using System.Collections;

public class WaveSeek : MonoBehaviour {
	public Tracer tracer;
	public SimpleWave wave;
	public Vector3 primaryDirection = Vector3.down;
	public float currentSpeed;
	public float amplitude;
	public float wavelength;
	public float waveStep;
	private Vector3 waveStartPoint;

	void Start()
	{
		/*if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}*/
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		if (tracer)
		{
			tracer.StartLine();
			waveStartPoint = transform.position;
		}
		if (wave == null)
		{
			wave = GetComponent<SimpleWave>();
		}
	}

	void Update()
	{
		if (primaryDirection.sqrMagnitude != 1)
		{
			primaryDirection.Normalize();
		}

		// TODO use speed to determine how far along the curve to move, not just in the primary direction.
		waveStep += currentSpeed * Time.deltaTime;
		if (waveStep > 1)
		{
			waveStep -= 1;
			waveStartPoint = transform.position;
		}
		transform.position = wave.FindWavePoint(primaryDirection, waveStartPoint, waveStep);
		
		if (tracer != null)
		{
			tracer.AddVertex(transform.position);
		}
	}
}
