using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour 
{
	#region Variables
	[Header("Set in Inspector")]
	public GameObject cloudSphere;
	public int numSphereMin = 6;
	public int numSphereMax = 10;
	public Vector3 sphereOffScale = new Vector3(5,2,1);
	public Vector2 sphereScaleRangeX = new Vector2 (4,8);
	public Vector2 sphereScaleRangeY = new Vector2 (3,4);
	public Vector2 sphereScaleRangeZ = new Vector2 (2,4);
	public float scaleYMin = 2f;

	private List<GameObject> spheres;	// The list of spheres holds reference to all the CloudSpheres that are instantiated by the Cloud
	#endregion

	#region UnityFunctions
	void Start()
	{
		spheres = new List<GameObject>();

		int num = Random.Range(numSphereMin, numSphereMax);	// Randomly choose how many CloudSpheres to attacj to this Cloud.
		for(int i = 0; i < num; i++)
		{
			GameObject sp = Instantiate<GameObject>(cloudSphere);	// each CloudSpheres is instantiated and added to spheres.
			spheres.Add(sp);
			Transform spTrans = sp.transform;
			spTrans.SetParent(this.transform);

			// Randomly assign a position
			Vector3 offset = Random.insideUnitSphere;	// A random point inside a unit sphere is chosen.
			offset.x *= sphereOffScale.x;
			offset.y *= sphereOffScale.y;
			offset.z *= sphereOffScale.z;
			spTrans.localPosition = offset;

			// Randomly assign scale
			Vector3 scale = Vector3.one;
			scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
			scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
			scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

			// Adjust y scale by x distance from core
			scale.y *= -(Mathf.Abs(offset.x)/sphereOffScale.x);
			scale.y = Mathf.Max(scale.y, scaleYMin);

			spTrans.localScale = scale;	// i

		}
	}

	void Update()
	{
//		if(Input.GetKeyDown(KeyCode.Space))
//		{
//			Restart();
//		}
	}

	void Restart()
	{
		// Clear out old spheres
		foreach(GameObject sp in spheres)
		{
			Destroy(sp);
		}
		Start();
	}
	#endregion

}// Main Class
