using UnityEngine;
using System.Collections;

public class Feedback : MonoBehaviour {

	public CameraShake cameraShake;
	public float cameraShakeFactor;
	public GameObject particleTrail;
	public GameObject colorExplosionPrefab;
	public ControllerFeedback controllerFeedback;
	private GameObject player;
	private GameObject pSys;
	private GameObject colExp;
	private Vector3 prevPos;
	private Vector3 currentDir;
	private Color startColor;
	private Color boostColorOne;
	private Color boostColorTwo;
	private Color boostColorThree;
	private Color boostColorFour;
	private int boostLevel = 0;
	private Tracer tracer;

	// Use this for initialization
	void Start () {
		if (cameraShake == null)
		{
			cameraShake = Camera.main.GetComponent<CameraShake>();
		}
		pSys = (GameObject)Instantiate(particleTrail);
		player = gameObject;
		prevPos = player.transform.position;
		startColor = player.renderer.material.color;
		boostColorOne = new Color(0.3f, 0.2f, 0.5f, 1.0f);
		boostColorTwo = new Color(0.3f, 0.6f, 0.3f, 1.0f);
		boostColorThree = new Color(0.95f, 0.5f, 0.0f, 1.0f);
		boostColorFour = new Color(1.0f, 1.0f, 0.0f, 1.0f);
		tracer = GetComponent<Tracer>();
	}
	
	// Update is called once per frame
	void Update () {
		pSys.transform.position = player.transform.position;
		pSys.particleSystem.startColor = player.renderer.material.color;

		currentDir = player.transform.position - prevPos;

		pSys.particleSystem.emissionRate = (currentDir.magnitude/Time.deltaTime)*2;

		currentDir.Normalize();


		if(currentDir.sqrMagnitude != 0)
			pSys.transform.rotation = Quaternion.LookRotation(-currentDir, pSys.transform.up);
		prevPos = player.transform.position;



		if(currentDir.magnitude <= 0.01f)
		{
			boostLevel = 0;
			player.renderer.material.color = startColor;
		}
	}

	void SpeedBoost()
	{
		if (cameraShake != null)
		{
			controllerFeedback.SetVibration(0.5f, 0.5f);
			cameraShake.ShakeCamera(cameraShakeFactor);
		}
		ChangeBoost(1);
	}

	void SpeedDrain()
	{
		ChangeBoost(-1);
	}

	void SpeedNormal()
	{
		if (cameraShake != null)
		{
			cameraShake.StopShaking();
		}
	}

	private void ChangeBoost(int levelChange)
	{
		boostLevel = Mathf.Clamp(boostLevel + levelChange, 0, 4);
		if (boostLevel == 4)
		{
			player.renderer.material.color = boostColorFour;
		}
		else if (boostLevel == 3)
		{
			player.renderer.material.color = boostColorThree;
		}
		else if (boostLevel == 2)
		{
			player.renderer.material.color = boostColorTwo;
		}
		else if (boostLevel == 1)
		{
			player.renderer.material.color = boostColorOne;
		}
		else if (boostLevel == 0)
		{
			player.renderer.material.color = startColor;
		}
		tracer.lineRenderer.material.color = player.renderer.material.color;
		colExp = (GameObject)Instantiate(colorExplosionPrefab);
		colExp.particleSystem.startColor = player.renderer.material.color;
		colExp.transform.position = player.transform.position;
		Destroy(colExp, 3.1f);
	}
}
