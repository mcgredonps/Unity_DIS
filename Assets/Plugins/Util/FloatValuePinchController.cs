using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// <summary>
// A class the modifies a floating point value based on 2 finger touches.
// </summary>
public class FloatValuePinchController : MonoBehaviour {
	
	public float pinchValue = 1.0f;
	public float pinchScaleFactor = 0.1f;
	public float pinchVelocityDecayTime = 0.0f; //< How many seconds it takes to go to 0
	public float min = 0.0f;
	public float max = 1.0f;
	
	private List<Vector2> lastTouches;
	
	private float initialPinchVelocity = 0.0f;
	private float currentPinchVelocity = 0.0f;
	private float lastPinchValue = 0.0f;
	private float decayTimeElapsed = 0.0f;

	// -------------------------------------------------------------------------
	void Start () {
	
		// Allocate list memory before starting coroutines that use them
		lastTouches = new List<Vector2>();
		
	}
	
	// -------------------------------------------------------------------------
	void Update () {
		
		if (Input.touches.Length == 2 && lastTouches.Count == 2) {
			
			float lastDistance = (lastTouches[0] - lastTouches[1]).magnitude;
			float currentDistance = (Input.touches[0].position - Input.touches[1].position).magnitude;
			
			// Calculate the current pinch value based on finger distance from last frame
			pinchValue += (currentDistance - lastDistance) * pinchScaleFactor;
			pinchValue = Mathf.Clamp(pinchValue, min, max);

			// Keep track of how fast we're pinching
			initialPinchVelocity = pinchValue - lastPinchValue;
			lastPinchValue = pinchValue;
			
			// We're still pinching, no decay
			decayTimeElapsed = 0.0f;
			
			
			
		} else {

			// Keep track of how much time we've been in decay for
			decayTimeElapsed += Time.deltaTime;
			
			float decayPercent = Mathf.Clamp(1.0f - decayTimeElapsed / pinchVelocityDecayTime, 0.0f, 1.0f);
			currentPinchVelocity = initialPinchVelocity * decayPercent;
			
			pinchValue += currentPinchVelocity;
			
		}
		
		lastTouches.Clear();
		
		// Store this frames touches to be used as lastTouches for the next frame
		foreach (Touch touch in Input.touches) {
			
			lastTouches.Add(touch.position);
		}	
	}
	
	// -------------------------------------------------------------------------
	public float GetVelocity() {
		
		return currentPinchVelocity;
	}
	
	// -------------------------------------------------------------------------
	public float GetInitialVelocity() {
		
		return initialPinchVelocity;
	}

	// -------------------------------------------------------------------------
	public bool IsPinching() {
		
		return Input.touches.Length == 2;
	}
}
