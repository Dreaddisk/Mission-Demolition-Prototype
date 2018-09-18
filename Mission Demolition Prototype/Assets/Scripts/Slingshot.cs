using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour 
{
	#region Variables
	[Header("Set in Inspector")]	
	public GameObject PrefabProjectile;
	public float velocityMult = 8f;

	[Header("Set Dynamically")]
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;
	
	private Rigidbody projectileRigidBody;
	#endregion

	#region UnityMethods
	void Awake()
	{
		Transform launchpointTrans = transform.Find("LaunchPoint");	// Searches for the LaunchPoint game Object.
		launchPoint = launchpointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchpointTrans.position;
	}

	void Update()
	{
		// If Slingshot is not in aimingMode, don't run this code
		if(!aimingMode)	return;

		// Get the current mouse position in 2D screen coordinates
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

		// Find the delta from the LaunchPos to the mousePos3D.
		Vector3 mousedelta = mousePos3D-launchPos;
		// Limit mousedelta to the radius of the slingshot SphereCollider
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if(mousedelta.magnitude > maxMagnitude)
		{
			mousedelta.Normalize();
			mousedelta *= maxMagnitude;
		}

		// Move the projectile to this new position
		Vector3 projPos = launchPos + mousedelta;
		projectile.transform.position = projPos;

		if(Input.GetMouseButtonUp(0))
		{
			// the mouse has been released
			aimingMode = false;
			projectileRigidBody.isKinematic = false;
			projectileRigidBody.velocity = -mousedelta * velocityMult;
			FollowCam.POI = projectile;
			projectile = null;
		}
	}

	#endregion
	
		void OnMouseEnter()
	{
//		print("Slingshot:OnMouseenter");
		launchPoint.SetActive(true);
	}

	void OnMouseExit()
	{
//		print("Slingshot:OnMouseExit");
		launchPoint.SetActive(false);
	}

	void OnMouseDown()
	{
		// Player has pressed the mouse button while over Slingshot
		aimingMode = true;
		// Instantiate a Projectile
		projectile = Instantiate(PrefabProjectile) as GameObject;
		// Start it at the LaunchPoint
		projectile.transform.position = launchPos;
		// Set it to isKinnematic for now
		projectile.GetComponent<Rigidbody>().isKinematic = true;
		projectileRigidBody = projectile.GetComponent<Rigidbody>();
		projectileRigidBody.isKinematic = true;
	}

}// Main Class
