using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tracer : MonoBehaviour {
	private LineRenderer lineRenderer = null;
	private int vertexCount = 0;
	private int stableVertexCount = 0;
	public float minDragToDraw = 1.0f;
	public GameObject lineMakerPrefab = null;
	private Vector3 almostLastVertex = Vector3.zero;
	private Vector3 lastVertex = Vector3.zero;
	private Vector3 lastDirection = Vector3.zero;

	void Start() {
		vertexCount = 0;
		stableVertexCount = 0;
	}

	public void StartLine(bool startAtVertex = false, Vector3 startVertex = new Vector3())
	{		
		// If this line is not on the required path, create a separate polyline.
		CreateLineMaker(true);
		
		AddVertex(transform.position);
	}

	public void AddVertex(Vector3 position) {	
		if (vertexCount > 1) {
			// Preserve look of the most recent line segement if the new vertex
			// drastically changes the direction of motion. Without this, the line segement
			// gets distorted as only the center point is ever stored.
			if (Vector3.Dot((position - lastVertex).normalized, lastDirection) < 0.6f) {
				lineRenderer.SetVertexCount(++vertexCount);
				Vector3 midPosition = lastVertex + (lastDirection * minDragToDraw / 4);
				lineRenderer.SetPosition(vertexCount - 1, midPosition); 
				lastVertex = midPosition;
			}
		}
		
		lineRenderer.SetVertexCount(++vertexCount);
		lineRenderer.SetPosition(vertexCount - 1, position); 
		lastDirection = (position - lastVertex).normalized;
		almostLastVertex = lastVertex;
		lastVertex = position;
	}	
	
	public void CreateLineMaker(bool criticalLine) {		
		GameObject newLineMaker = (GameObject)Instantiate(lineMakerPrefab, Vector3.zero, Quaternion.identity);
		newLineMaker.transform.parent = transform;
		lineRenderer = newLineMaker.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(0);
		vertexCount = 0;
	}
}
