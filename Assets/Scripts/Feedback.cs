using UnityEngine;
using System.Collections;

public class Feedback : MonoBehaviour {

	public GameObject pSysPrefab;
	public GameObject colorExplosionPrefab;
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
	private bool exploded = false;
	private int boostLevel = 0;
	private Tracer tracer;

	// Use this for initialization
	void Start () {
		pSys = (GameObject)Instantiate(pSysPrefab);
		player = GameObject.FindGameObjectWithTag("Converser");
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
		//pSys.particleSystem.renderer.material.color = player.renderer.material.color;



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

	void SpeedBoost() {
		if(boostLevel == 3)
		{
			player.renderer.material.color = boostColorFour;
			boostLevel++;
		}
		else if(boostLevel == 2)
		{
			player.renderer.material.color = boostColorThree;
			boostLevel++;
		}
		else if(boostLevel == 1)
		{
			player.renderer.material.color = boostColorTwo;
			boostLevel++;
		}
		else if(boostLevel == 0)
		{
			player.renderer.material.color = boostColorOne;
			boostLevel++;
		}
		tracer.lineRenderer.material.color = player.renderer.material.color;
		colExp = (GameObject)Instantiate(colorExplosionPrefab);
		colExp.particleSystem.startColor = player.renderer.material.color;
		colExp.transform.position = player.transform.position;
		Destroy(colExp, 3.1f);
	}
}
