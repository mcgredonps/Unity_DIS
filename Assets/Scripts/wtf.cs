using UnityEngine;
using System.Collections;

public class wtf : MonoBehaviour {

    #region Public Fields
    #endregion

    #region Private Fields
    #endregion

    #region Properties
    #endregion

    #region MonoBehaviour Methods

    // -------------------------------------------------------------------------
    void Start () {
		
		transform.position = new Vector3(-1.0f, 0.0f, 0.0f);
		Vector3 test = new Vector3();
		test.Set (10.0f, 0.0f, 0.0f);
    }

    // -------------------------------------------------------------------------
    void Update () {
		
		transform.position.Set(10.0f, 0.0f, 0.0f);
    }

    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}
