using UnityEngine;
using System.Collections;

#pragma warning disable 0649

public class Util_Gyro : MonoBehaviour {	
	
	private Quaternion quatMult;	

	// -------------------------------------------------------------------------
	void Start () {
		
		// check if this transform has a parent
		Transform originalParent = transform.parent; 
		
		// make a new parent
		GameObject camParent = new GameObject ("camParent"); 
		
		// move the new parent to this transform position
		camParent.transform.position = transform.position; 
		
		// make this transform a child of the new parent
		transform.parent = camParent.transform; 
		
		// make the new parent a child of the original parent
		camParent.transform.parent = originalParent; 
		
		if (SystemInfo.supportsGyroscope) {
			Input.gyro.enabled = true;
			
		#if UNITY_IPHONE
			camParent.transform.eulerAngles = new Vector3(90f,90f,0f);
			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
				quatMult = new Quaternion(0f,0f,0.7071f,0.7071f);
			} else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
 				quatMult = new Quaternion(0f,0f,-0.7071f,0.7071f);
        	} else if (Screen.orientation == ScreenOrientation.Portrait) {
            	quatMult = new Quaternion(0f,0f,1f,0f);
			} else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
            	quatMult = new Quaternion(0f,0f,0f,1f);
			}
		#endif
		#if UNITY_ANDROID
			camParent.transform.eulerAngles = new Vector3(-90f,0f,0f);
			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
				//quatMult = Quaternion(0f,0,0.7071,-0.7071);
				quatMult = new Quaternion(0f,0f,0f,1f);
			} else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
  				//quatMult = Quaternion(0,0,-0.7071,-0.7071);
  				quatMult = new Quaternion(0f,0f,1f,0f);
        	} else if (Screen.orientation == ScreenOrientation.Portrait) {
            	quatMult = new Quaternion(0f,0f,0f,1f);
			} else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
            	quatMult = new Quaternion(0f,0f,1f,0f);
			}	
		#endif
		}		
	}
	
	// -------------------------------------------------------------------------
	public Vector3 GetGyroForward() {
		
		#if UNITY_IPHONE
			Quaternion quatMap = Input.gyro.attitude;
		
			// Change the local rotation so we can pull the correct value from transform.forward
			transform.localRotation = quatMap * quatMult;
		#endif
		#if UNITY_ANDROID
			Quaternion quatMap = new Quaternion(Input.gyro.attitude.w,Input.gyro.attitude.x,Input.gyro.attitude.y,Input.gyro.attitude.z);
		
			// Change the local rotation so we can pull the correct value from transform.forward
			transform.localRotation = quatMap * quatMult;
		#endif		
		
		
		return transform.forward;
	}	
	
}
