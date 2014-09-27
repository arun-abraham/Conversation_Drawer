using UnityEngine;
using System.Collections;

public class ConversingSpeed : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	public float boostRate;
	public float drainRate;
	private float targetSpeed;
	private BoostStatus boostStatus;
	private bool draining;
	private float boostLeft;
	private float boostIncrement;
	private float startMaxSpeed;

	public enum BoostStatus
	{
		STABLE = 0,
		BOOST,
		DRAIN
	}

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (mover != null)
		{
			startMaxSpeed = mover.maxSpeed;
		}
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
	}

	void Update()
	{
		if (boostStatus != BoostStatus.STABLE)
		{
			boostLeft -= Time.deltaTime;
			mover.maxSpeed = mover.maxSpeed + (boostIncrement * Time.deltaTime);
			bool boostEnded = false;
			if (boostStatus == BoostStatus.BOOST)
			{
				if (mover.maxSpeed >= targetSpeed)
				{
					mover.maxSpeed = targetSpeed;
					boostEnded = true;
				}
				if (boostEnded || boostLeft <= 0)
				{
					SendMessage("SpeedBoostEnded", SendMessageOptions.DontRequireReceiver);
					boostEnded = true;
				}
			}
			else
			{
				//mover.maxSpeed = Mathf.Min(mover.maxSpeed - (mover.maxSpeed * drainRate), 0);
			}

			if (boostEnded)
			{
				boostLeft = 0;
				boostIncrement = 0;
				boostStatus = BoostStatus.STABLE;
			}
		}
	}

	public void TargetRelativeSpeed(float boostPercentage)
	{
		targetSpeed = mover.maxSpeed * (1 + boostPercentage);
		TargetAbsoluteSpeed(targetSpeed);
	}

	public void TargetAbsoluteSpeed(float targetSpeed)
	{
		InterruptSpeedChange();
		boostStatus = BoostStatus.BOOST;
		if (targetSpeed >= mover.maxSpeed)
		{
			boostLeft = 1 / boostRate;
			boostIncrement = (targetSpeed - mover.maxSpeed) * boostRate;
		}
		else
		{
			boostLeft = 1 / drainRate;
			boostIncrement = (targetSpeed - mover.maxSpeed) * boostRate;
		}
	}

	private void InterruptSpeedChange()
	{
		bool interrupted = false;
		switch (boostStatus)
		{
			case BoostStatus.BOOST:
				SendMessage("SpeedBoostInterrupted", SendMessageOptions.DontRequireReceiver);
				interrupted = true;
				break;
			case BoostStatus.DRAIN:
				SendMessage("SpeedDrainInterrupted", SendMessageOptions.DontRequireReceiver);
				interrupted = true;
				break;
		}

		if (interrupted)
		{
			SendMessage("SpeedChangeInterrupted", SendMessageOptions.DontRequireReceiver);
		}
	}
}
