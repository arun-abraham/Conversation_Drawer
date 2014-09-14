using UnityEngine;
using System.Collections;

public class WaveSeek : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	public Tracer tracer;
	public float partnerWeight;
	public SimpleWave wave;
	public Vector3 primaryDirection = Vector3.down;
	public float waveDistanceTravelled;
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
		partnerWeight = Mathf.Clamp(partnerWeight, 0, 1);

		if (primaryDirection.sqrMagnitude != 1)
		{
			primaryDirection.Normalize();
		}

		if (partnerLink.Partner != null)
		{
			if (partnerLink.Leading)
			{
				if (wave.arcResetable)
				{
					waveDistanceTravelled = 0;
					wave.arcResetable = false;
				}
				waveDistanceTravelled += mover.maxSpeed * Time.deltaTime;
				float estimateTime = wave.ApproximateWaveTime(primaryDirection, waveStartPoint, waveDistanceTravelled);
				Vector3 destination = wave.FindWavePoint(primaryDirection, waveStartPoint, estimateTime);

				Vector3 fromPartner = transform.position - partnerLink.Partner.transform.position;
				fromPartner.z = 0;
				Vector3 fromPast = destination - transform.position;
				fromPast.z = 0;

				if (partnerWeight > 0 && Vector3.Dot(fromPartner, fromPast) <= 0)
				{
					Vector3 waveFollowChange = fromPast * (1 - partnerWeight);

					Vector3 considerPartnerChange = fromPartner.normalized * mover.maxSpeed * partnerWeight * Time.deltaTime;
					destination = transform.position + waveFollowChange + considerPartnerChange;
				}

				mover.Accelerate(destination - transform.position);

				if (tracer != null)
				{
					tracer.AddVertex(transform.position);
				}
			}
			else
			{
				mover.Accelerate(partnerLink.Partner.transform.position - transform.position);
				if (tracer != null)
				{
					tracer.AddVertex(transform.position);
				}
			}
		}
		else
		{
			mover.SlowDown();
		}
	}
}
