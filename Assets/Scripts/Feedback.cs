using UnityEngine;
using System.Collections;

public class Feedback : MonoBehaviour {

	public GameObject pSysPrefab;
	public GameObject colorExplosionPrefab;
	private GameObject player;
	private GameObject pSys;
	private GameObject colExp;
	private Color currentColor;
	private Vector3 prevPos;
	private Vector3 currentDir;
	private Color boostColorOne;
	private Color boostColorTwo;
	private Color boostColorThree;
	private Color boostColorFour;
	private bool exploded = false;

	// Use this for initialization
	void Start () {
		pSys = (GameObject)Instantiate(pSysPrefab);
		player = GameObject.FindGameObjectWithTag("Player");
		prevPos = player.transform.position;
		boostColorOne = new Color(0.3f, 0.5f, 0.3f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		pSys.transform.position = player.transform.position;
		currentColor = player.renderer.material.color;
		pSys.particleSystem.startColor = currentColor;



		currentDir = player.transform.position - prevPos;

		pSys.particleSystem.emissionRate = currentDir.magnitude/Time.deltaTime;

		currentDir.Normalize();


		if(currentDir.sqrMagnitude != 0)
			pSys.transform.rotation = Quaternion.LookRotation(-currentDir, pSys.transform.up);
		prevPos = player.transform.position;

		if(Input.GetKeyDown(KeyCode.A))
		{
			player.renderer.material.color = boostColorOne;
			colExp = (GameObject)Instantiate(colorExplosionPrefab);
			colExp.particleSystem.startColor = player.renderer.material.color;
			exploded = true;
		}
		if(exploded == true)
			colExp.transform.position = player.transform.position;
	}
}
