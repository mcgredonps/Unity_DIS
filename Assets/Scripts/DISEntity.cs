using UnityEngine;
using System.Collections;

public class DISEntity : MonoBehaviour {

    #region Public Fields
	public ushort entityID;	
    #endregion

    #region Private Fields
	OpenDis.Dis1998.EntityStatePdu lastPdu;
    #endregion

    #region MonoBehaviour Methods

    // -------------------------------------------------------------------------
    void Start () {

    }

    // -------------------------------------------------------------------------
    void Update () {

    }

    #endregion

    #region Public Methods
	// -------------------------------------------------------------------------
	void OnNewPduReceived(OpenDis.Dis1998.EntityStatePdu pdu) {
		
		if (pdu.EntityID.Entity == entityID) {
			
		} else {
			Debug.LogError("Entity received pdu with invalid id.");
		}
	}
    #endregion

    #region Private Methods
    #endregion
}
