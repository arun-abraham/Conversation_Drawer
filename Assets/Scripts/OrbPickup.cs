using UnityEngine;
using System.Collections;

public class OrbPickup : MonoBehaviour {

	public GameObject largeExplosionPrefab;
	public GameObject colorfulTrailPrefab;
	private GameObject largeExplosion;
	private GameObject colorfulTrail;
	private Feedback feedback;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Converser")
		{
			feedback = col.gameObject.GetComponent<Feedback>();
			feedback.AlternateTrail();
			largeExplosion = (GameObject)Instantiate(largeExplosionPrefab);
			largeExplosion.transform.position = col.transform.position;

			Destroy(gameObject);
		}
	}
}
