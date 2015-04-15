using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Finger 
{
	private int? id;	// nullable int
	// TOUCH : finger.id = touch.id + 1
	// MOUSE : finger.id = 0
	// UNINITIALIZED : finger.id = null

	// Recorded previous positions for velocity/path calculation
	private List<Vector2> prevWorldPositions = new List<Vector2>();
	private List<Vector2> prevScreenPositions = new List<Vector2>();
	private List<float> prevTimes = new List<float>();

	// The times that this finger started and ended (became invalid)
	private float? startTime = null, endTime = null;
	private bool isValid, isFresh;

	// The object that was hit when this touch was initiated 
	private GameObject startHitObject;

	// The number of finger positions to record for velocity/path calculations
	private const int recordCount = 10;

	// If this finger was initiated by a touch, stores that touch object
	private Touch touch;

	// Empty constructor
	public Finger()
	{
		this.SetId(null);
		this.SetValidity(false);
		this.FindStartHitObject();
	}

	// Mouse constructor
	public Finger(Vector2 mousePosition)
	{
		this.SetId(0);
		this.SetValidity(true);
		this.UpdatePrevPositionLists();
		this.FindStartHitObject();
		this.isFresh = true;
	}

	// Touch constructor
	public Finger(Touch touch) 
	{
		this.SetTouch(touch);
		this.SetTouchId(touch);
		this.SetValidity(true);
		this.FindStartHitObject();
		this.isFresh = true;
	}

	// Updates this finger's recorded previous positions and draws the debug ray
	public void Update()
	{
		this.UpdatePrevPositionLists();
		this.Draw();

		if (this.isFresh)
		{
			this.isFresh = false;
		}
	}

	// Updates this touchfinger's touch
	public void Update(Touch touch)
	{
		this.touch = touch;

		this.Update();
	}

	// Updates the lists of recorded previous positions
	private void UpdatePrevPositionLists()
	{
		// don't allow list of previous positions to be longer than 10
		if (prevWorldPositions.Count > recordCount-1)
		{
			prevWorldPositions.RemoveAt(0);
			prevScreenPositions.RemoveAt(0);
			prevTimes.RemoveAt(0);
		}

		// update lists of previous input positions
		prevWorldPositions.Add(this.GetWorldPosition());
		prevScreenPositions.Add(this.GetScreenPosition());
		// update list of previous input times
		prevTimes.Add(Time.time);
	}

	// Set the current validity of this finger
	public void SetValidity(bool validity)
	{
		if (validity)
		{
			this.isValid = true;
			this.SetStartTime();
		}
		else
		{
			this.isValid = false;
			this.SetEndTime();
		}
	}

		// Sets the time that this finger was initialized
		// SHOULD ONLY BE CALLED UPON INITIALIZATION
		private void SetStartTime()
		{
			this.startTime = Time.time;
		}

		// Sets the time that this finger became invalid
		// SHOULD ONLY BE CALLED WHEN FINGER BECOMES INVALID
		private void SetEndTime()
		{
			if (this.startTime != null)
			{
				this.endTime = Time.time;
			}
		}

	// Gets the current validity of this finger
	public bool GetValidity()
	{
		return this.isValid;
	}

	// Sets the id of this finger
	private void SetId(int? id)
	{
		this.id = id;
	}

		// Sets the id of this finger given a touch
		private void SetTouchId(Touch touch)
		{
			this.SetId(touch.fingerId + 1);
		}

	// Gets the id of this finger's touch
	public int GetTouchId()
	{
		return (int)((this.id) - 1);
	}

		// Gets the nullable id of this touch
		public int? GetId()
		{
			return this.id;
		}

	// Returns whether this finger is a touch
	public bool isTouch()
	{
		return (this.GetId() != null && this.GetId() > 0);
	}

	// Returns whether this finger is a mouse
	public bool isMouse()
	{
		return (this.GetId() != null && this.GetId() == 0);
	}

	// Returns whether this finger is new
	public bool isNew()
	{
		return this.isFresh;
	}

	// Sets the touch that initialized this finger
	private void SetTouch(Touch touch)
	{
		this.touch = touch;
	}

	// Gets the touch that initialized this finger
	public Touch GetTouch()
	{
		return this.touch;
	}

	// Gets the game object currently being hit by the finger
	private GameObject GetCurrentHitObject()
	{
		RaycastHit2D hit = Physics2D.Raycast(GetRayOrigin(), Vector2.zero);
		if (hit.collider != null)
		{
			return hit.collider.gameObject;
		}

		return null;
	}
	// Finds the object that this finger hit upon initialization
	// SHOULD ONLY BE CALLED DURING INITIALIZATION
	private void FindStartHitObject()
	{
		this.startHitObject = GetCurrentHitObject();
	}

	// Gets the object this finger was hitting when it was initalized
	public GameObject GetStartHitObject()
	{
		return this.startHitObject;
	}

	// Gets whether this finger is currently hitting a specific game object
	public bool isHittingObject(GameObject go)
	{
		return (go != null && go == this.GetCurrentHitObject());
	}

	// Gets whether this finger was initialized hitting a specific game object
	public bool startedHittingOject(GameObject go)
	{
		return (go != null && go == this.GetStartHitObject());
	}

	// If this finger is valid, 					returns the duration of this finger since it was initialized
	// If this finger was valid but is now invalid, returns the duration of this finger during which it was valid
	// If this finger was never valid, 				returns null
	public float GetDuration()
	{
		float duration = 0f;

		if (this.GetValidity())
		{
			duration = (float)(Time.time - startTime);
		}
		else
		{
			if ((endTime - startTime) != null)
			{
				duration = (float)(endTime - startTime);
			}
		}

		return duration;
	}

	// Gets the total duration of the recording of previous finger positions
	private float GetVelocityDuration()
	{
		return (prevTimes[0] - prevTimes[prevTimes.Count-1]);
	}

	// Gets the position of this finger in 2D screen coordinates
	public Vector2 GetScreenPosition() 
	{
		Vector2 screenPos = Vector2.zero;

		if (this.isTouch())
		{
			Touch fingerTouch = this.GetTouch();
			screenPos = new Vector2(fingerTouch.position.x, fingerTouch.position.y);
		}
		else if (this.isMouse())
		{
			screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);	
		}

		return screenPos;
	}

		// Gets the position of this finger in 3D screen coordinates
		public Vector3 GetScreenPosition3() 
		{
			Vector2 screenPos = this.GetScreenPosition();
			return new Vector3(screenPos.x, screenPos.y, 0f);
		}

	// Gets the position of this finger in 2D world coordinates
	public Vector2 GetWorldPosition() 
	{
		Vector2 screenPos = this.GetScreenPosition();
		Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));

		return worldPos;
	}

		// Gets the position of this finger in 3D world coordinates
		public Vector3 GetWorldPosition3() 
		{
			Vector2 worldPos = this.GetWorldPosition();
			return new Vector3(worldPos.x, worldPos.y, 0f);
		}

	// Gets the 2D origin of a ray starting from the camera's plane and ending at this finger's current world position	
	private Vector2 GetRayOrigin()
	{
		Vector2 rayOrigin = this.GetWorldPosition();

		return rayOrigin;

	}
		// Gets the 3D origin of a ray starting from the camera's plane and ending at this finger's current world position	
		private Vector3 GetRayOrigin3()
		{
			Vector3 rayOrigin = this.GetWorldPosition3();
			rayOrigin = new Vector3(rayOrigin.x, rayOrigin.y, Camera.main.transform.position.z);

			return rayOrigin;
		}

	// Calculate finger screen velocity
	public Vector2 GetScreenVelocity()
	{
		return this.GetVelocity(prevScreenPositions);
	}
		
	// Calculate finger world velocity
	public Vector2 GetWorldVelocity() 
	{
		return this.GetVelocity(prevWorldPositions);
	}

	// Calculate finger world/screen velocity
	private Vector2 GetVelocity(List<Vector2> prevPositions)
	{
		Vector2 velocity = Vector2.zero;
		if (prevPositions.Count > 1)
		{
			Vector2 sumDeltas = Vector2.zero;
			for (int i = 1; i < prevPositions.Count; i++)
			{
				sumDeltas += prevPositions[i] - prevPositions[i-1];
			}
			//sumDeltas += this.GetWorldPosition() - prevPositions[prevPositions.Count-1];

			velocity = sumDeltas / this.GetVelocityDuration();
		}

		return velocity;
	}

	// Draws this finger's ray from the camera
	// Green if valid, red if invalid
	public void Draw()
	{
		if (this.GetValidity())
		{
			Debug.DrawRay(this.GetRayOrigin3(), Vector3.forward * 2f, Color.green);
		}
		else
		{
			Debug.DrawRay(this.GetRayOrigin3(), Vector3.forward * 2f, Color.red);
		}
	}

	public override string ToString() 
	{
		string output = "";
		output += "Finger " + this.GetId() + " ";

		Vector2 worldPos = this.GetWorldPosition();
		output += "@ {" + worldPos.x + ", " + worldPos.y + "} ";


		Vector2 worldVel = this.GetWorldVelocity();
		output += " with velocity {" + worldVel.x + ", " + worldVel.y + "}";

		return output;
	}
}