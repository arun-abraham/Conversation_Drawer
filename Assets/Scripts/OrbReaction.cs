using UnityEngine;
using System.Collections;

public class OrbReaction : MonoBehaviour {
	private bool onTrip;
	public bool OnTrip
	{
		get { return onTrip; }
	}
	public float tripDuration;
	private float startTripDuration;
	public float tripDurationDampening;
	private float tripElapsed;
	public float tripBoost = 0.3f;
	private SimpleMover mover;
	private Color startColor;
	public Renderer headRenderer;
	public Color minColor = new Color(0.2f, 0.2f, 0.2f, 1);

	void Start()
	{
		mover = GetComponent<SimpleMover>();
		startColor = headRenderer.material.color;
		startTripDuration = tripDuration;
	}

	void Update()
	{
		if (onTrip)
		{
			tripElapsed += Time.deltaTime;
			Color colorFade = (Color.white - startColor) * (Time.deltaTime / tripDuration * (startTripDuration/ tripDuration));
			headRenderer.material.color -= colorFade;
			headRenderer.material.color = new Color(Mathf.Max(headRenderer.material.color.r, minColor.r), Mathf.Max(headRenderer.material.color.g, minColor.g), Mathf.Max(headRenderer.material.color.b, minColor.b), 1);
			if (tripElapsed >= tripDuration)
			{
				tripElapsed = 0;
				onTrip = false;
				mover.externalSpeedMultiplier -= tripBoost;
				GetComponent<Feedback>().DestroyAlternateTrail();
			}
		}
	}

	public bool StartTrip()
	{
		if (!onTrip)
		{
			onTrip = true;
			tripElapsed = 0;
			mover.externalSpeedMultiplier += tripBoost;
			tripDuration *= tripDurationDampening;
			headRenderer.material.color = Color.white;
			return true;
		}
		return false;
	}
}
