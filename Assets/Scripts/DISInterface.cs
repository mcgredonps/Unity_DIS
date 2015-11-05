using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using OpenDis.Dis1998;

public class DISInterface : MonoBehaviour {

    #region Public Fields
	public Transform testEntity;
    #endregion

    #region Private Fields
	int PDU_TYPE_POSITION = 2;
	bool _receivedFirstEntity = false;	
	
	UDPThread _udpThread = null;
	Dictionary<ushort, Transform> _entityMap = new Dictionary<ushort, Transform>();
	
	Vector2d _originLatLon = new Vector2d();
	Vector2d _metersPerDegreeLatLon = new Vector2d();
    #endregion

    #region Properties
    #endregion

    #region MonoBehaviour Methods

    // -------------------------------------------------------------------------
    void Start () {
		_udpThread = GetComponentInChildren<UDPThread>();
		_udpThread.eventDataReceived += new Action<object, byte[]>(OnDataReceived);
    }

    // -------------------------------------------------------------------------
    void Update () {
	
		
    }

    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
	// -------------------------------------------------------------------------
	void OnDataReceived(object o, byte[] data) {
		
		try {
			
			OpenDis.Enumerations.PduType pduType = (OpenDis.Enumerations.PduType)data[PDU_TYPE_POSITION];
		
			OpenDis.Dis1998.Pdu pdu = 
				OpenDis.Core.PduProcessor.UnmarshalRawPdu(pduType, data, OpenDis.Core.Endian.Big);
			
			//Debug.Log(pdu.GetType().ToString());
			
			ProcessPdu(pdu);
			
		} catch (Exception e) {
			Debug.LogError(e.ToString());
		}
	}
	
	// -------------------------------------------------------------------------
	void ProcessPdu(OpenDis.Dis1998.Pdu pdu) {
		
		switch ((OpenDis.Enumerations.PduType)pdu.PduType) {
		case OpenDis.Enumerations.PduType.EntityState:
			ProcessEntityStatePdu(pdu as OpenDis.Dis1998.EntityStatePdu);
			break;
		default:
			break;
		}
	}
	
	// -------------------------------------------------------------------------
	void ProcessEntityStatePdu(OpenDis.Dis1998.EntityStatePdu pdu) {
		
		// If this is the first item we've received, map its latlon to (0,0)
		if (!_receivedFirstEntity) {			
			_originLatLon.x = pdu.EntityLocation.X;
			_originLatLon.y = pdu.EntityLocation.Y;
			Util_GPS.GetMetersPerDegree(ref _originLatLon, out _metersPerDegreeLatLon.x, out _metersPerDegreeLatLon.y);
			_receivedFirstEntity = true;
		}
		
		// See if we've already created an object for this entity
		Transform entityTransform = null;
		_entityMap.TryGetValue(pdu.EntityID.Entity, out entityTransform);
		
		// If we don't have an object in the scene representing this id, create one
		if (entityTransform == null) {			
			entityTransform = (Transform)Instantiate(testEntity, Vector3.zero, Quaternion.identity);
			_entityMap.Add(pdu.EntityID.Entity, entityTransform);			
		}
		
		double x = pdu.EntityLocation.X;
		double y = pdu.EntityLocation.Y;
		double z = pdu.EntityLocation.Z;
		
		double lat, lon, alt;
		
		Util_GPS.GeocentricToGeodetic(x, y, z, out lat, out lon, out alt);
		Debug.Log(String.Format("{3}: received entity state: ({0}, {1}, {2})", lat, lon, alt, pdu.EntityType.Specific));
	}
    #endregion
	
	#region Testing
	// -------------------------------------------------------------------------
	void ProcessEntityStatePduLatLon(OpenDis.Dis1998.EntityStatePdu pdu) {
		
		// If this is the first item we've received, map its latlon to (0,0)
		if (!_receivedFirstEntity) {			
			_originLatLon.x = pdu.EntityLocation.X;
			_originLatLon.y = pdu.EntityLocation.Y;
			Util_GPS.GetMetersPerDegree(ref _originLatLon, out _metersPerDegreeLatLon.x, out _metersPerDegreeLatLon.y);
			_receivedFirstEntity = true;
		}
		
		// See if we've already created an object for this entity
		Transform entityTransform = null;
		_entityMap.TryGetValue(pdu.EntityID.Entity, out entityTransform);
		
		// If we don't have an object in the scene representing this id, create one
		if (entityTransform == null) {			
			entityTransform = (Transform)Instantiate(testEntity, Vector3.zero, Quaternion.identity);
			_entityMap.Add(pdu.EntityID.Entity, entityTransform);			
		}
		
		double worldX = (pdu.EntityLocation.X - _originLatLon.x) * _metersPerDegreeLatLon.x;
		double worldY = (pdu.EntityLocation.Y - _originLatLon.y) * _metersPerDegreeLatLon.y;
			
		entityTransform.position = new Vector3((float)worldX, (float)pdu.EntityLocation.Z, (float)worldY);
		 
		Debug.Log(String.Format("{3}: received entity state: ({0}, {1}, {2})", pdu.EntityLocation.X,
				pdu.EntityLocation.Y, pdu.EntityLocation.Z, pdu.EntityType.Specific));
	}
	#endregion
}
