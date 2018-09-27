using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour 
{
	#region Variables
	public float easing = 0.05f;
	public Vector2 minXY = Vector2.zero;
	static public GameObject POI; // the static point of interest 
	//POI is the point of interest that the camera should follow.

	[Header("Set Dynamically")]
	public float camZ; // The desired Z pos of the camera

	#endregion

	#region UnityMethods
	void Awake()
	{
		camZ = this.transform.position.z;
	}

	void FixedUpdate()
	{
		// If there is one line following an if, it doesn't need braces
//		if(POI == null) return;	// if POI is set to null, non of teh rest of the code is executed.

		Vector3 destination;
		// if there is no poi, return to P:[0,0,]
		if(POI == null)
		{
			destination = Vector3.zero;
		}
		else
		{
			// Get the position of the poi
			destination = POI.transform.position;
			// if poi is a Projectile, check to see if it's at rest
			if(POI.tag == "Projectile")
			{
				// if it is sleeping (that is, not moving)
				if(POI.GetComponent<Rigidbody>().IsSleeping())
				{
					// return to default view
					POI = null;
					// in the next update
					return;
				}
			}
		}
		 // get the position of the poi

//		 Vector3 destination = POI.transform.position;
		 // Interpolate from teh current Camera position toward destination

		 destination = Vector3.Lerp(transform.position, destination, easing);
		 // Limit the X & Y to minimum values
		 destination.x = Mathf.Max(minXY.x, destination.x);
		 destination.y = Mathf.Max(minXY.y, destination.y);
		 // Force destination.z to be camZ to keep the camera far enough away
		 destination.z = camZ;
		 // Set the camera to the destination
		 transform.position = destination;
		 // Set the orthographicSize of the Camera to keep Ground in view.
		 Camera.main.orthographicSize = destination.y + 10;
	}

	#endregion
}// Main Class
