﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour 
{
	#region Variables
	static public ProjectileLine S;	// Singleton

	[Header("Set in Inspector")]
	public float minDist = 0.1f;

	private LineRenderer line;
	private GameObject _poi;
	private List<Vector3> points;
	#endregion


	#region UnityFunctions
	void Awake()
	{
		S = this; // Set the singleton
		// Get a reference to teh LineRenderer
		line = GetComponent<LineRenderer>();
		// Disable the LineRenderer until it's needed
		line.enabled = false;
		// Initialize the points List
		points = new List<Vector3>();
	}

	#endregion

	// This is a propertu (that is, a method masquerading as a field)
	public GameObject poi
	{
		get
		{
			return(_poi);
		}
		set
		{
			_poi = value;
			if(_poi != null)
			{
				// When _poi is set to something new, it resets everything
				line.enabled = false;
				points = new List <Vector3>();
				AddPoint();
			}
		}
	}

	// This can be used to clear the line directly
	public void Clear()
	{
		_poi = null;
		line.enabled = false;
		points = new List<Vector3>();
	}

	public void AddPoint()
	{
		// This is called to add a point to the line
		Vector3 pt = _poi.transform.position;
		if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
		{
			// If the point isn't far enough from the Last point, it returns
			return;
		}

		if(points.Count == 0)
		{
			Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // to be defined
			//... it adds an extra bit of line to aod aiming later
			points.Add(pt + launchPosDiff);
			points.Add(pt);
			line.positionCount = 2;
			// Sets the first two points
			line.SetPosition(0, points[0]);
			line.SetPosition(1, points[1]);
			// Enables the lineRenderer
			line.enabled = true;
		}
		else
		{
			// Normal behaviour of adding a point
			points.Add(pt);
			line.positionCount = points.Count;
			line.SetPosition(points.Count-1, lastPoint);
			line.enabled = true;
		}
	}

	// Returns the Location of the most recently added point
	public Vector3 lastPoint
	{
		get
		{
			if(points == null)
			{
				// if there are no points, returns Vector3.zero
				return (Vector3.zero);
			}
			return(points[points.Count-1]);
		}
	}

	void FixedUpdate()
	{
		if(poi == null)
		{
			// If there is no poi, search for one
			if(FollowCam.POI != null)
			{
				if(FollowCam.POI.tag == "Projectile")
				{
					poi = FollowCam.POI;
				}
				else
				{
					return; // Return if we didn't find a poi
				}
			}
			else
			{
				return; // Return if we didn't find a poi
			}
		}
		// If there is a poi, it's a Loc is added every FixedUpdate
		AddPoint();
		if(FollowCam.POI == null)
		{
			// Once FollowCam.POI is null, make the Local poi null too
			poi = null;
		}
	}
}
