using UnityEngine;
using System.Collections;

public class Helper {
	public static Vector3 ProjectVector(Vector3 baseDirection, Vector3 projectee)
	{
		if (baseDirection.sqrMagnitude != 1)
		{
			baseDirection.Normalize();
		}
		
		float projecteeMag = projectee.magnitude;
		float projecteeDotBase = 0;
		if (projecteeMag > 0)
		{
			projecteeDotBase = Vector3.Dot(projectee / projecteeMag, baseDirection);
		}
		Vector3 projection = baseDirection * projecteeMag * projecteeDotBase;
		return projection;
	}
}
