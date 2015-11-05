using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;


public class NetworkReceiver : MonoBehaviour {
	
	class UdpState {
		public IPEndPoint endPoint;
		public UdpClient client;
	}

    #region Public Fields
	public string multicastGroup = "226.226.226.226";
	public string IP = "127.0.0.1";	
    public int port=6391;

	public event Action<object, byte[]> eventDataReceived = null;
    #endregion

    #region Private Fields
	IPEndPoint anyIP;
	UdpClient client;
	UdpState udpState;
	IAsyncResult asyncResult = null;
	
	List<byte[]> dataList;
    #endregion

    #region Properties
    #endregion

    #region MonoBehaviour Methods

    // -------------------------------------------------------------------------
    void Start () {
		
		dataList = new List<byte[]>();
		
		try {
			
			anyIP = new IPEndPoint(IPAddress.Any, port);
			client = new UdpClient(anyIP);
			
			client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			
			//client.JoinMulticastGroup(IPAddress.Parse(multicastGroup));
			
			ListenForData();
			
		} catch (SocketException e) {			
			Debug.Log(e.ToString());
		} 
    }

    // -------------------------------------------------------------------------
    void Update () {
		
		foreach(byte[] data in dataList) {
			
			if (eventDataReceived != null) {
				eventDataReceived(this, data);	
			}
			
			Debug.Log(System.Text.Encoding.UTF8.GetString(data));
		}
		
		dataList.Clear();
    }

    #endregion

    #region Private Methods
	
	// -------------------------------------------------------------------------
	void ListenForData() {

		try 
        {
			udpState = new UdpState();
			udpState.endPoint = anyIP;
			udpState.client = client;			
			
            asyncResult = client.BeginReceive(new System.AsyncCallback(OnDataReceived), udpState); 
        }
        catch (System.Exception err) 
        {		
            print(err.ToString());
        }
	}
	
	// -------------------------------------------------------------------------
	void OnDataReceived(System.IAsyncResult result) {
		
		udpState = ((UdpState)result.AsyncState);
		
		System.Byte[] receivedBytes = udpState.client.EndReceive(result, ref udpState.endPoint);
		dataList.Add(receivedBytes);
		
		// keep listeneing
		//ListenForData();
	}
	
	// -------------------------------------------------------------------------
	void OnApplicationQuit() {
		
		if (client != null) {
			
			try {
				client.EndReceive(asyncResult, ref anyIP);
			}
			catch (Exception e) {
				print (e.ToString());
			}
			
			client.Close();
		}
	}
	
    #endregion
}
