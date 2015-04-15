using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pad : MonoBehaviour 
{
	public NetworkPlayer owner;
	public int id;

	public Color color;
	public float radius = 0.75f;

	public Vector2 position;
	public Vector2 velocity;

	public Finger finger;
	public bool isHeld;
	public bool isDead;
	
	public float warmth = 0.0f;
	private float lastWarmth;
	
	private bool fade = true;

	void Awake () {}
	
	void Update () 
	{
		// if this is MY pad
		if (this.transform.parent.GetComponent<PlayerStateManager>().isMe)	// HACK
		{
			
			Debug.Log("network player " + Network.player);
			Debug.Log("owner " + owner);
			
			if (isHeld) 
			{
				if (finger != null && finger.GetValidity())
				{
					position = finger.GetWorldPosition();
					velocity = finger.GetWorldVelocity();
					if(warmth > 0f && fade) warmth -=0.001f;
					color = Color.Lerp(Color.black, Color.red, warmth);
				}
				else 
				{
					//isDead = true;
				}
			}
		}

		this.transform.position = new Vector3(this.position.x, this.position.y, 0f);
		//this.transform.localScale = Vector3.one * radius;
		this.GetComponent<SpriteRenderer>().color = color;
	}

	public Vector2 GetPosition() 
	{
		return this.position;
	}

	public Vector2 GetVelocity() 
	{
		return this.velocity;
	}

	public void SetPosition(Vector2 pos) 
	{
		this.position = pos;
	}

	public void SetVelocity(Vector2 vel) 
	{
		this.velocity = vel;
	}
	
	public void SetColor(Color c){
		color = c;
	}

	public void AddVelocity(Vector2 vel) 
	{
		this.velocity += vel;
	}

	public Vector3 GetPosition3() 
	{
		return new Vector3(this.position.x, this.position.y, 0f);
	}

	public Vector3 GetVelocity3() 
	{
		return new Vector3(this.velocity.x, this.velocity.y, 0f);
	}

	public void SetPosition3(Vector3 pos) 
	{
		this.position = new Vector2(pos.x, pos.y);
	}

	public void SetVelocity3(Vector3 vel) 
	{
		this.velocity = new Vector2(vel.x, vel.y);
	}

	public void AddVelocity3(Vector3 vel) 
	{
		this.velocity += new Vector2(vel.x, vel.y);
	}

	public void Hold(Finger finger) 
	{
		this.isHeld = true;
		this.finger = finger;
	}
	
	void OnTriggerEnter2D(Collider2D c){
		if(c.GetComponent<Pad>()) {
			fade = false;
		}
	}
	
	void OnTriggerStay2D(Collider2D c){
		Debug.Log("collide");
		if(c.GetComponent<Pad>()) {
			if( warmth < 1.1f) warmth += 0.1f;
		}
	}
	
	void OnTriggerExit2D(Collider2D c){
		if(c.GetComponent<Pad>()) {
			fade = true;
		}
	}
}
