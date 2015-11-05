using UnityEngine;
using System.Collections;

public class Util_TouchReport : MonoBehaviour {
	
	public int lastTouchCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
//		if (Input.GetMouseButtonDown(0)) {
//			Debug.Log("Mouse 0 down");
//		}
//		
//		if (Input.GetMouseButtonDown(1)) {
//			Debug.Log("Mouse 1 down");
//		}
//		
//		if (Input.GetMouseButtonUp(0)) {
//			Debug.Log("Mouse 0 up");
//		}
//		
//		if (Input.GetMouseButtonUp(1)) {
//			Debug.Log("Mouse 1 up");
//		}
//		
//		if (Input.touches.Length > 0) {
//			
//			if (Input.touches[0].phase == TouchPhase.Began) {
//				Debug.Log("touch began");
//			}
//			
//			if (Input.touches[0].phase == TouchPhase.Ended) {
//				Debug.Log("touch ended");
//			}
//		}
		
		if (Input.touches.Length != lastTouchCount) {
			Debug.Log("touch count: " + Input.touches.Length);
			lastTouchCount = Input.touches.Length;
		}
	}
}
