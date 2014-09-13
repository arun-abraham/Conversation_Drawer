using UnityEngine;
using System.Collections;

public class WaveSeek : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	public Tracer tracer;
	public SimpleWave wave;
	public Vector3 primaryDirection = Vector3.down;
	public float distanceTravelled;
	private Vector3 waveStartPoint;

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
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

		if (partnerLink.Partner != null)
		{
			distanceTravelled += mover.maxSpeed * Time.deltaTime;
			float estimateTime = wave.ApproximateWaveTime(primaryDirection, waveStartPoint, distanceTravelled);
			mover.MoveTo(wave.FindWavePoint(primaryDirection, waveStartPoint, estimateTime));
			
			if (tracer != null)
			{
				tracer.AddVertex(transform.position);
			}
		}
	}
}
