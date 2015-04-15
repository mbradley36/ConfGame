using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestureHandler : MonoBehaviour 
{
	public static GestureHandler instance;
	public int fingerCount = 0;
	public List<Finger> fingers = new List<Finger>();
	public Finger mouseFinger;
	public bool debugFingers;

	private bool allowTouches = true, allowMouse = false;

	void Start()
	{
		instance = this;
		mouseFinger = new Finger(Input.mousePosition);
	}

	void Update() 
	{
		List<Touch> newTouches = new List<Touch>();

		// Update existing Fingers using reported touches
		if (allowTouches)
		{
			// Populate list of all current touches
			foreach (Touch touch in Input.touches) 
			{
				newTouches.Add(touch);
			}

			// Update all the touchfingers
			foreach (Finger finger in fingers) 
			{
				if (finger.isTouch())
				{
					bool found = false;
					foreach (Touch touch in Input.touches) 
					{
						if (touch.fingerId == finger.GetTouchId()) 
						{
							found = true;
							finger.Update(touch);
							newTouches.Remove(touch);
						}
					}

					if (!found) 
					{
						finger.SetValidity(false);
					}
				}
			}

			// Make new Fingers for remaining unaccounted-for touches
			foreach (Touch touch in newTouches) 
			{
				Finger newFinger = new Finger(touch);
				fingers.Add(newFinger);
			}

			// Prevent mouse input if there are touches
			if (Input.touchCount > 0)
			{
				allowMouse = false;
			}
			else
			{
				allowMouse = true;
			}

			CleanFingers();
		}
		
		if (allowMouse)
		{
			// Update the mousefinger
			if (Input.GetMouseButton(0) && mouseFinger != null)
			{
				mouseFinger.Update();
			}

			if (Input.GetMouseButtonDown(0))
			{
				fingers.Clear();
				allowTouches = false;
				mouseFinger = new Finger(Input.mousePosition);
				fingers.Add(mouseFinger);
			} 
			else if (Input.GetMouseButtonUp(0))
			{
				allowTouches = true;
				mouseFinger.SetValidity(false);
				fingers.Remove(mouseFinger);
			}
		}

		PrintAllFingers();

		this.fingerCount = fingers.Count;
	}

	void PrintAllFingers() 
	{
		if (debugFingers)
		{
			if (fingerCount > 0) 
			{
				string output = "";
				output += "+-----------" + fingerCount + " Fingers-----------+\n";
				foreach (Finger finger in fingers) 
				{
					output += "|\t" + finger.ToString() + "\n";
				}
				output += "+--------------------------------+";
				print(output);
			} 
			else 
			{
				//Debug.Log("########### 0 Fingers ###########");
			}
		}
	}

	void CleanFingers() 
	{
		// Clean up invalid Fingers
		List<Finger> valids = new List<Finger>();
		foreach (Finger finger in fingers)
		{
			if (finger.GetValidity())
			{
				valids.Add(finger);
			}
		}

		fingers = valids;
	}
}
