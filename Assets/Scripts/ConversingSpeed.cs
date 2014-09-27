using UnityEngine;
using System.Collections;

public class ConversingSpeed : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
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
					SendMessage("EndSpeedBoost", SendMessageOptions.DontRequireReceiver);
					boostEnded = true;
				}
			}
			else
			{
				if (mover.maxSpeed <= targetSpeed)
				{
					mover.maxSpeed = targetSpeed;
					boostEnded = true;
				}
				if (boostEnded || boostLeft <= 0)
				{
					SendMessage("EndSpeedDrainEnd", SendMessageOptions.DontRequireReceiver);
					boostEnded = true;
				}
			}

			if (boostEnded)
			{
				boostLeft = 0;
				boostIncrement = 0;
				boostStatus = BoostStatus.STABLE;
				SendMessage("EndSpeedChange", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void TargetRelativeSpeed(float boostPercentage, float changeRate)
	{
		targetSpeed = mover.maxSpeed * (1 + boostPercentage);
		TargetAbsoluteSpeed(targetSpeed, changeRate);
	}

	public void TargetAbsoluteSpeed(float targetSpeed, float changeRate)
	{
		InterruptSpeedChange();

		this.targetSpeed = targetSpeed;

		if (changeRate <= 0)
		{
			boostLeft = 0;
			boostIncrement = 0;
			boostStatus = BoostStatus.STABLE;
			SendMessage("EndSpeedChange", SendMessageOptions.DontRequireReceiver);
			return;
		}

		if (targetSpeed >= mover.maxSpeed)
		{
			boostLeft = 1 / changeRate;
			boostIncrement = (targetSpeed - mover.maxSpeed) * changeRate;
			boostStatus = BoostStatus.BOOST;
		}
		else
		{

			boostLeft = 1 / changeRate;
			boostIncrement = (targetSpeed - mover.maxSpeed) * changeRate;
			boostStatus = BoostStatus.DRAIN;
		}

	}

	private void InterruptSpeedChange()
	{
		bool interrupted = false;
		switch (boostStatus)
		{
			case BoostStatus.BOOST:
				SendMessage("InterruptSpeedBoost", SendMessageOptions.DontRequireReceiver);
				SendMessage("EndSpeedBoost", SendMessageOptions.DontRequireReceiver);
				interrupted = true;
				break;
			case BoostStatus.DRAIN:
				SendMessage("InterruptSpeedDrain", SendMessageOptions.DontRequireReceiver);
				SendMessage("EndSpeedDrain", SendMessageOptions.DontRequireReceiver);
				interrupted = true;
				break;
		}

		if (interrupted)
		{
			SendMessage("InterruptSpeedChange", SendMessageOptions.DontRequireReceiver);
			SendMessage("EndSpeedChange", SendMessageOptions.DontRequireReceiver);
		}
	}
}
