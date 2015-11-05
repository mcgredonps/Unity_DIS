using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;

public class NetworkInterface : MonoBehaviour {

    #region Public Fields
	public NetworkSender netSender = null;
	public UDPThread udpThread = null;
    #endregion

    #region Private Fields
	string receiveMulticastString = "226.226.226.226";
	string receiveIPString = "127.0.0.1";
	string receiverPortString = "6390";
	
	string sendIPString = "127.0.0.1";
	string sendPortString = "6390";
	string lastPacketText = "";
	string textToSend = "some default text";
    #endregion

    #region MonoBehaviour Methods

    // -------------------------------------------------------------------------
    void Start () {
		
		udpThread.eventDataReceived += new Action<object, byte[]>(OnDataReceived);		
		AcquireIP();
    }
	
	// -------------------------------------------------------------------------
	void OnGUI() {
		
		GUILayout.BeginArea(new Rect(325, 30, 250, 100));
			GUILayout.Label("Multicast Address");
			GUILayout.TextField(receiveMulticastString, GUILayout.Width(200));
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(100, 100, 250, 100));		
			GUILayout.Label("Receive Address");
			GUILayout.BeginHorizontal();
			GUILayout.Label("IP");
			GUILayout.TextField(receiveIPString, GUILayout.Width(200));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Port");
			GUILayout.TextField(receiverPortString, GUILayout.Width(200));
			GUILayout.EndHorizontal();		
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(100, 200, 300, 300));
			GUILayout.TextArea(lastPacketText, GUILayout.Height(300));
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(450, 100, 250, 100));
			GUILayout.Label("Send Address");
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("IP");
			sendIPString = GUILayout.TextField(sendIPString, GUILayout.Width(200));
			GUILayout.EndHorizontal();
		
			GUILayout.BeginHorizontal();
			GUILayout.Label("Port");
			sendPortString = GUILayout.TextField(sendPortString, GUILayout.Width(200));
			GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(450, 200, 300, 300));
			textToSend = GUILayout.TextArea(textToSend, GUILayout.Height(300));
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(705, 125, 100, 100));
			if (GUILayout.Button("Send", GUILayout.Height(45), GUILayout.Width(45))) {
				SendData(textToSend);
			}
		GUILayout.EndArea();
	}

    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
	
	// -------------------------------------------------------------------------
	void OnDataReceived(object sender, byte[] data) {
		
		lastPacketText = System.Text.Encoding.UTF8.GetString(data);
	}
	
	// -------------------------------------------------------------------------
	void SendData(string data) {
			
		int port = System.Convert.ToInt32(sendPortString);		
		netSender.SendData(sendIPString, port, System.Text.Encoding.UTF8.GetBytes(data));
	}
	
	// -------------------------------------------------------------------------
	void AcquireIP() {
		
		IPHostEntry host;
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
		    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
		    {
		        receiveIPString = ip.ToString();
				break;
		    }
		}
	}
    #endregion
}
