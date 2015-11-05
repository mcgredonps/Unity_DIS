using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;


public class NetworkSender : MonoBehaviour {

    #region Public Methods
	
	// -------------------------------------------------------------------------
	public void SendData(string ipString, int port, byte[] data) {
		
		UdpClient client = null;
		
		try 
        {
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipString), port);
			client = new UdpClient(endPoint);
			
            client.Send(data, data.Length, endPoint);
        }
        catch (System.Exception err) 
        {
            print(err.ToString());
        }
		finally
		{
			if (client != null) {
				client.Close();	
			}
			
		}
	}
	
    #endregion

}
