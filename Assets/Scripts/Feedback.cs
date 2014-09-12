using UnityEngine;
using System.Collections;

public class Feedback : MonoBehaviour {

	public GameObject pSysPrefab;
	private GameObject player;
	private GameObject pSys;
	private Color currentColor;

	// Use this for initialization
	void Start () {
		pSys = (GameObject)Instantiate(pSysPrefab);
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		pSys.transform.parent = player.transform;
		pSys.transform.localPosition = new Vector3(0, 0, 0);
		currentColor = player.renderer.material.color;
		pSys.particleSystem.startColor = currentColor;
	}
}
